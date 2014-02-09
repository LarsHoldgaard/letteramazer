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
        private LetterAmazerEntities Repository;
        private ILetterService letterService;
        private IPaymentService paymentService;
        private ICouponService couponService;
        private ICustomerService customerService;

        public OrderService(LetterAmazerEntities repository,
            ILetterService letterService, IPaymentService paymentService,
            ICouponService couponService,IOrderFactory orderFactory, ICustomerService customerService)
        {
            this.Repository = repository;
            this.letterService = letterService;
            this.paymentService = paymentService;
            this.couponService = couponService;
            this.orderFactory = orderFactory;
            this.customerService = customerService;

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

            foreach (var orderLine in order.OrderLines)
            {
                var dbOrderLine = new DbOrderItems();
                dbOrderLine.Quantity = orderLine.Quantity;
                dbOrderLine.ItemType = (int) orderLine.ProductType;
                dbOrderLine.OrderId = order.Id;

                if (orderLine.ProductType == ProductType.Order)
                {
                    var letter = ((Letter)orderLine.BaseProduct);

                    DbLetters dbLetter = new DbLetters()
                    {
                        CustomerId = letter.Customer.Id,
                        FromAddress_Address = letter.FromAddress.Address1,
                        FromAddress_Address2 = letter.FromAddress.Address2,
                        FromAddress_AttPerson = letter.FromAddress.AttPerson,
                        FromAddress_City = letter.FromAddress.City,
                        FromAddress_Co = letter.FromAddress.Co,
                        FromAddress_CompanyName = string.Empty,
                        FromAddress_Country = letter.FromAddress.Country.Id,
                        FromAddress_FirstName = letter.FromAddress.FirstName,
                        FromAddress_LastName = letter.FromAddress.LastName,
                        FromAddress_Postal = letter.FromAddress.PostalCode,
                        FromAddress_State = letter.FromAddress.State,
                        FromAddress_VatNr = letter.FromAddress.VatNr,
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
                    dbOrderLine.DbLetters = dbLetter;
                }


                dborder.DbOrderItems.Add(dbOrderLine);
            }

            Repository.SaveChanges();

            return GetOrderById(order.Id);
        }

        public Order Update(Order order)
        {
            var dborder = Repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);

            if (dborder == null)
            {
                throw new BusinessException("Order is null");
            }

            dborder.Guid = order.Guid;
            dborder.OrderCode = order.OrderCode;
            dborder.OrderStatus = (int)order.OrderStatus;
            dborder.DateUpdated = DateTime.Now;
            
            Repository.SaveChanges();

            return GetOrderById(order.Id);
        }

        public List<Order> GetOrderBySpecification(OrderSpecification specification)
        {
            IQueryable<DbOrders> dbOrders = Repository.DbOrders;

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

            var ord = dbOrders.Skip(specification.Skip).Take(specification.Take).ToList();
            return orderFactory.Create(ord);
        }

        public Order GetOrderById(int orderId)
        {
            DbOrders dborder = Repository.DbOrders.FirstOrDefault(c => c.Id == orderId);
            if (dborder == null)
            {
                throw new ItemNotFoundException("Order");
            }
            var order = orderFactory.Create(dborder);

            return order;
        }

        public void Delete(Order order)
        {
            var dborder = Repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);
            Repository.DbOrders.Remove(dborder);
            Repository.SaveChanges();

        }

        public List<OrderLine> GetOrderLinesBySpecification(OrderLineSpecification specification)
        {
            IQueryable<DbOrderItems> dbOrderItems = Repository.DbOrderItems;

            if (specification.OrderId > 0)
            {
                dbOrderItems = dbOrderItems.Where(c => c.OrderId == specification.OrderId);
            }
            

            return orderFactory.Create(
                dbOrderItems.Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public void DeleteOrder(Order order)
        {
            var dborder = Repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);

            Repository.DbOrders.Remove(dborder);
            Repository.SaveChanges();
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

        #region Private helper methods
        private string GenerateOrderCode()
        {
            string orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();

            if (Repository.DbOrders.Any(c => c.OrderCode == orderCode))
            {
                orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();
            }

            return orderCode;
        }

        #endregion


    }
}
