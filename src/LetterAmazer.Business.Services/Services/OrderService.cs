﻿using System.Data.Entity.Validation;
using System.Reflection;
using System.Web.UI.WebControls.WebParts;
using LetterAmazer.Business.Services.Domain.Caching;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Factory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Utils;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Data.Repository.Data;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services
{
    public class OrderService : IOrderService
    {
        private IOrderFactory orderFactory;
        
        private LetterAmazerEntities repository;
        private ILetterService letterService;
        private IMailService mailService;
        private ICustomerService customerService;
        private IPartnerService partnerService;
        private ICacheService cacheService;
        private IOrganisationService organisationService;

        public OrderService(LetterAmazerEntities repository,
            ILetterService letterService,
            IOrderFactory orderFactory, ICustomerService customerService, IPartnerService partnerService,
            ICacheService cacheService, IMailService mailService,IOrganisationService organisationService)
        {
            this.repository = repository;
            this.letterService = letterService;
            this.orderFactory = orderFactory;
            this.customerService = customerService;
            this.partnerService = partnerService;
            this.cacheService = cacheService;
            this.mailService = mailService;
            this.organisationService = organisationService;
        }

        public Order Create(Order order)
        {
            DbOrders dborder = new DbOrders();


            try
            {
                foreach (var orderLine in order.OrderLines)
                {
                    var dbOrderLine = setOrderline(orderLine);
                    dborder.DbOrderlines.Add(dbOrderLine);
                }

                dborder.Guid = Guid.NewGuid();
                dborder.OrderCode = order.OrderCode ?? Helpers.GetRandomInt(1000, 99999999).ToString(); // don't set it here, but use checkout contrller (problem sovled with credit)

                if (order.Customer.AccountStatus == AccountStatus.Test)
                {
                    dborder.OrderStatus = (int) OrderStatus.Test;
                }
                else
                {
                    dborder.OrderStatus = (int) order.OrderStatus;
                }



                dborder.DateCreated = DateTime.Now;
                dborder.DateUpdated = DateTime.Now;
                dborder.OrganisationId = order.OrganisationId;
                dborder.CustomerId = order.Customer != null ? order.Customer.Id : 0;

                Price price = new Price();
                price.PriceExVat = order.CostFromLines();
                price.VatPercentage = order.Customer.VatPercentage();
                order.Price = price;

                dborder.Total = order.Price.Total;
                dborder.VatPercentage = order.Price.VatPercentage;
                dborder.PriceExVat = order.Price.PriceExVat;

                repository.DbOrders.Add(dborder);
                repository.SaveChanges();

            }
            catch (DbEntityValidationException exe)
            {
                int i = 0;
            }
            
            foreach(var partnerTransaction in order.PartnerTransactions)
            {
                partnerTransaction.OrderId = dborder.Id;
                partnerService.Create(partnerTransaction);
            }

            cacheService.DeleteByContaining("GetOrderBySpecification");

            mailService.NotificationNewOrder(dborder.Total.ToString());

            return GetOrderById(dborder.Id);
        }
        
        public Order Update(Order order)
        {
            var dborder = repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);

            if (dborder == null)
            {
                throw new BusinessException("Order is null");
            }

            dborder.Guid = order.Guid;
            dborder.OrderCode = order.OrderCode;
            dborder.OrderStatus = (int)order.OrderStatus;
            dborder.DateUpdated = DateTime.Now;
            dborder.DatePaid = order.DatePaid;
            dborder.DateSent = order.DateSent;
            dborder.CustomerId = order.Customer.Id;
            dborder.PriceExVat = order.Price.PriceExVat;
            dborder.Total = order.Price.Total;
            dborder.VatPercentage = order.Price.VatPercentage;
            dborder.OrganisationId = order.OrganisationId;

            repository.SaveChanges();

            cacheService.DeleteByContaining("GetOrderBySpecification");
            return GetOrderById(order.Id);
        }

        public List<Order> GetOrderBySpecification(OrderSpecification specification)
        {
            var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, specification.ToString());
            if (!cacheService.ContainsKey(cacheKey))
            {
                IQueryable<DbOrders> dbOrders = repository.DbOrders;

                if (specification.OrderStatus.Any())
                {
                    dbOrders = dbOrders.Where(c => specification.OrderStatus.Contains((OrderStatus)c.OrderStatus));
                }
                if (specification.FromDate != null)
                {
                    dbOrders = dbOrders.Where(c => c.DateCreated >= specification.FromDate);
                }
                if (specification.ToDate != null)
                {
                    dbOrders = dbOrders.Where(c => c.DateCreated <= specification.ToDate);
                }
                if (specification.UserId > 0)
                {
                    var customer = customerService.GetCustomerById(specification.UserId);

                    // if administator or poweruser, they can see all orders in the organisation
                    if (customer.OrganisationRole.HasValue &&
                        (customer.OrganisationRole.Value == OrganisationRole.Administrator ||
                         customer.OrganisationRole.Value == OrganisationRole.Poweruser))
                    {
                        dbOrders = dbOrders.Where(c => c.OrganisationId == customer.Organisation.Id ||
                            c.CustomerId == specification.UserId);
                    }
                    else
                    {
                        dbOrders = dbOrders.Where(c => c.CustomerId == specification.UserId);
                    }
                }

                var ord = dbOrders.OrderByDescending(c => c.DateCreated).Skip(specification.Skip).Take(specification.Take).ToList();

                List<List<DbOrderlines>> dbOrderItems = ord.
                    Select(dbOrderse => repository.DbOrderlines.Where(c => c.OrderId == dbOrderse.Id).
                        ToList()).ToList();
                var res= orderFactory.Create(ord, dbOrderItems);
                cacheService.Create(cacheKey, res);
                return res;
            }
            return (List<Order>) (cacheService.GetById(cacheKey));
        }

        public Order GetOrderById(Guid orderId)
        {
            DbOrders dborder = repository.DbOrders.FirstOrDefault(c => c.Guid == orderId);
            if (dborder == null)
            {
                throw new ItemNotFoundException("Order");
            }

            var lines = repository.DbOrderlines.Where(c => c.OrderId == dborder.Id).ToList();
            var order = orderFactory.Create(dborder, lines);

            return order;
        }

        public Order GetOrderById(int orderId)
        {
            DbOrders dborder = repository.DbOrders.FirstOrDefault(c => c.Id == orderId);
            if (dborder == null)
            {
                throw new ItemNotFoundException("Order");
            }

            var lines = repository.DbOrderlines.Where(c => c.OrderId == orderId).ToList();
            var order = orderFactory.Create(dborder, lines);

            return order;
        }

        public void Delete(Order order)
        {
            var dborder = repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);
            repository.DbOrders.Remove(dborder);
            repository.SaveChanges();

        }

        public List<OrderLine> GetOrderLinesBySpecification(OrderLineSpecification specification)
        {
            IQueryable<DbOrderlines> dbOrderItems = repository.DbOrderlines;

            if (specification.OrderId > 0)
            {
                dbOrderItems = dbOrderItems.Where(c => c.OrderId == specification.OrderId);
            }
            

            return orderFactory.Create(
                dbOrderItems.Skip(specification.Skip).Take(specification.Take).ToList());
        }

        /// <summary>
        /// When an order turns to paid, this runs on every orderline (for instance providing credits)
        /// </summary>
        /// <param name="order"></param>
        public void ReplenishOrderLines(Order order)
        {
            foreach (var orderLine in order.OrderLines)
            {
                if (orderLine.ProductType == ProductType.Credit)
                {
                    var credits = orderLine.Price.Total;
                    var organisation = order.Customer.Organisation;
                    organisation.Credit += credits;
                    organisationService.Update(organisation);
                }
            }
        }

        public void DeleteOrder(Order order)
        {
            var dborder = repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);

            repository.DbOrders.Remove(dborder);
            repository.SaveChanges();
        }

        public void UpdateByLetters(IEnumerable<Letter> letters)
        {
            foreach (var letter in letters)
            {
                letter.LetterStatus = LetterStatus.Sent;
                letterService.Update(letter);

                var order = GetOrderById(letter.OrderId);
                
                bool isOrderDone = true;

                foreach (var orderLine in order.OrderLines)
                {
                    // if this is the case, there are multiple lines and one of them is not sent yet, which means the order is in progress
                    if (orderLine.ProductType == ProductType.Letter && 
                        ((Letter)orderLine.BaseProduct).LetterStatus == LetterStatus.Created)
                    {
                        isOrderDone = false;
                    }
                }
                if (isOrderDone)
                {
                    order.OrderStatus = OrderStatus.Done;
                }
                else
                {
                    order.OrderStatus = OrderStatus.InProgress;
                }
                Update(order);
            }
        }

        public OrderLine GetOrderlineById(int orderLineId)
        {
            var orderLineDb = repository.DbOrderlines.FirstOrDefault(c => c.Id == orderLineId);

            if (orderLineDb == null)
            {
                throw new BusinessException("Orderline doesn't exist");
            }

            return orderFactory.Create(orderLineDb);
        }

        #region Private helper methods

        private DbOrderlines setOrderline(OrderLine orderLine)
        {
            var dbOrderLine = new DbOrderlines();
            dbOrderLine.Quantity = orderLine.Quantity;
            dbOrderLine.ItemType = (int)orderLine.ProductType;
            dbOrderLine.PriceExVat = orderLine.Price.PriceExVat;
            dbOrderLine.VatPercentage = orderLine.Price.VatPercentage;
            dbOrderLine.Total = orderLine.Price.Total;

            if (orderLine.ProductType == ProductType.Payment && orderLine.PaymentMethodId >0)
            {
                dbOrderLine.PaymentMethodId = orderLine.PaymentMethodId;
            }
            if (orderLine.ProductType == ProductType.Letter)
            {
                var letter = ((Letter)orderLine.BaseProduct);

                // TODO: move logic to letter... or reuse one in letter?
                DbLetters dbLetter = new DbLetters()
                {
                    CustomerId = letter.CustomerId,
                    OrganisationId = letter.OrganisationId,
                    ToAddress_Address = letter.ToAddress.Address1,
                    ToAddress_Address2 = letter.ToAddress.Address2,
                    ToAddress_AttPerson = letter.ToAddress.AttPerson,
                    ToAddress_City = letter.ToAddress.City,
                    ToAddress_Co = letter.ToAddress.Co,
                    ToAddress_CompanyName = string.Empty,
                    ToAddress_Country = letter.ToAddress.Country.Id,
                    ToAddress_FirstName = letter.ToAddress.FirstName,
                    ToAddress_LastName = letter.ToAddress.LastName,
                    ToAddress_Zipcode = letter.ToAddress.Zipcode,
                    ToAddress_State = letter.ToAddress.State,
                    ToAddress_VatNr = letter.ToAddress.VatNr,
                    OrderId = letter.OrderId,
                    LetterContent_WrittenContent = letter.LetterContent.WrittenContent,
                    LetterContent_Path = letter.LetterContent.Path,
                    LetterStatus = (int)letter.LetterStatus,
                    OfficeId = letter.OfficeId,
                    LetterColor = (int)letter.LetterDetails.LetterColor,
                    LetterPaperWeight = (int)letter.LetterDetails.LetterPaperWeight,
                    LetterProcessing = (int)letter.LetterDetails.LetterProcessing,
                    LetterSize = (int)letter.LetterDetails.LetterSize,
                    LetterType = (int)letter.LetterDetails.LetterType,
                    ReturnLabel = letter.ReturnLabel,
                    DeliveryLabel = (int)letter.DeliveryLabel,
                    Guid = Guid.NewGuid()
                };

                if (letter.FromAddress != null)
                {
                    dbLetter.FromAddress_Address = letter.FromAddress.Address1;
                    dbLetter.FromAddress_Address2 = letter.FromAddress.Address2;
                    dbLetter.FromAddress_AttPerson = letter.FromAddress.AttPerson;
                    dbLetter.FromAddress_City = letter.FromAddress.City;
                    dbLetter.FromAddress_Co = letter.FromAddress.Co;
                    dbLetter.FromAddress_CompanyName = string.Empty;

                    if (letter.FromAddress.Country != null && letter.FromAddress.Country.Id > 0)
                    {
                        dbLetter.FromAddress_Country = letter.FromAddress.Country.Id;
                    }

                    dbLetter.FromAddress_FirstName = letter.FromAddress.FirstName;
                    dbLetter.FromAddress_LastName = letter.FromAddress.LastName;
                    dbLetter.FromAddress_Zipcode = letter.FromAddress.Zipcode;
                    dbLetter.FromAddress_State = letter.FromAddress.State;
                    dbLetter.FromAddress_VatNr = letter.FromAddress.VatNr;
                }
                
                dbOrderLine.DbLetters = dbLetter;
            }
            return dbOrderLine;
        }


        #endregion


    }
}
