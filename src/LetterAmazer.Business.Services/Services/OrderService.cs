using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Factory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Data.Repository.Data;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services
{
    public class OrderService : IOrderService
    {
        private IOrderFactory orderFactory;
        
        private LetterAmazerEntities repository;
        private ILetterService letterService;

        public OrderService(LetterAmazerEntities repository,
            ILetterService letterService,
            IOrderFactory orderFactory)
        {
            this.repository = repository;
            this.letterService = letterService;
            this.orderFactory = orderFactory;
        }

        public Order Create(Order order)
        {
            DbOrders dborder = new DbOrders();
            dborder.Guid = Guid.NewGuid();
            dborder.OrderCode = GenerateOrderCode();
            dborder.OrderStatus = (int)OrderStatus.Created;
            dborder.DateCreated = DateTime.Now;
            dborder.DateUpdated = DateTime.Now;
            dborder.Cost = 0m;
            dborder.PaymentMethod = "";
            dborder.Discount = 0.0m;
            dborder.Price = 0.0m;
            dborder.CustomerId = order.Customer != null ? order.Customer.Id : 0;

            foreach (var orderLine in order.OrderLines)
            {
                var dbOrderLine = setOrderline(orderLine);
                dborder.DbOrderlines.Add(dbOrderLine);
            }

            repository.DbOrders.Add(dborder);

            repository.SaveChanges();

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
            
            repository.SaveChanges();

            return GetOrderById(order.Id);
        }

        public List<Order> GetOrderBySpecification(OrderSpecification specification)
        {
            IQueryable<DbOrders> dbOrders = repository.DbOrders;

            if (specification.OrderStatus.Any())
            {
                dbOrders = dbOrders.Where(c => specification.OrderStatus.Contains((OrderStatus) c.OrderStatus));
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
                dbOrders = dbOrders.Where(c => c.CustomerId == specification.UserId);
            }

            var ord = dbOrders.OrderBy(c=>c.Id).Skip(specification.Skip).Take(specification.Take).ToList();

            List<List<DbOrderlines>> dbOrderItems = ord.
                Select(dbOrderse => repository.DbOrderlines.Where(c => c.OrderId == dbOrderse.Id).
                    ToList()).ToList();
            return orderFactory.Create(ord, dbOrderItems);
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
                    if (orderLine.ProductType == ProductType.Order && ((Letter)orderLine.BaseProduct).LetterStatus == LetterStatus.Created)
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
        private string GenerateOrderCode()
        {
            string orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();

            if (repository.DbOrders.Any(c => c.OrderCode == orderCode))
            {
                orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();
            }

            return orderCode;
        }

        private DbOrderlines setOrderline(OrderLine orderLine)
        {
            var dbOrderLine = new DbOrderlines();
            dbOrderLine.Quantity = orderLine.Quantity;
            dbOrderLine.ItemType = (int)orderLine.ProductType;
            dbOrderLine.Cost = orderLine.Cost; 

            if (orderLine.ProductType == ProductType.Payment && orderLine.PaymentMethod != null)
            {
                dbOrderLine.PaymentMethodId = orderLine.PaymentMethod.Id;
                dbOrderLine.CouponId = orderLine.CouponId;
            }
            if (orderLine.ProductType == ProductType.Order)
            {
                var letter = ((Letter)orderLine.BaseProduct);

                DbLetters dbLetter = new DbLetters()
                {
                    ToAddress_Address = letter.ToAddress.Address1,
                    ToAddress_Address2 = letter.ToAddress.Address2,
                    ToAddress_AttPerson = letter.ToAddress.AttPerson,
                    ToAddress_City = letter.ToAddress.City,
                    ToAddress_Co = letter.ToAddress.Co,
                    ToAddress_CompanyName = string.Empty,
                    ToAddress_Country = letter.ToAddress.Country.Id,
                    ToAddress_FirstName = letter.ToAddress.FirstName,
                    ToAddress_LastName = letter.ToAddress.LastName,
                    ToAddress_Postal = letter.ToAddress.PostalCode,
                    ToAddress_State = letter.ToAddress.State,
                    ToAddress_VatNr = letter.ToAddress.VatNr,
                    OrderId = letter.OrderId,
                    LetterContent_WrittenContent = letter.LetterContent.WrittenContent,
                    LetterContent_Content = letter.LetterContent.Content,
                    LetterContent_Path = letter.LetterContent.Path,
                    LetterStatus = (int)letter.LetterStatus,
                    OfficeProductId = letter.OfficeProductId,
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
                    dbLetter.FromAddress_Postal = letter.FromAddress.PostalCode;
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
