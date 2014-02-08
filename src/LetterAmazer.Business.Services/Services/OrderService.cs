using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Services.PaymentMethods;
using LetterAmazer.Data.Repository.Data;
using LinqKit;
using System.Threading;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services
{
    public class OrderService : IOrderService
    {
        private OrderFactory orderFactory;
        private LetterAmazerEntities Repository;
        private ILetterService letterService;
        private IPaymentService paymentService;
        private ICouponService couponService;
        private ICustomerService customerService;

        public OrderService(LetterAmazerEntities repository,
            ILetterService letterService, IPaymentService paymentService,
            ICouponService couponService,OrderFactory orderFactory, ICustomerService customerService)
        {
            this.Repository = repository;
            this.letterService = letterService;
            this.paymentService = paymentService;
            this.couponService = couponService;
            this.orderFactory = orderFactory;
            this.customerService = customerService;

        }

        public string CreateOrder(OrderContext orderContext)
        {
            DbOrders order = new DbOrders();
            order.OrderType = (int)OrderType.SendLetter;
            order.Guid = Guid.NewGuid().ToString();
            order.OrderCode = GenerateOrderCode();
            order.OrderStatus = (int)OrderStatus.Created;
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.Cost = 0m;
            foreach (var orderItem in orderContext.Order.Letters)
            {
                Repository.DbOrderItems.Add(new DbOrderItems()
                {
                    DbOrders = order,
                    DbLetters = null,
                    Price = 0.0m
                });
                //repository.Create(orderItem.Letter);
                //orderItem.Price = letterService.GetCost(orderItem.Letter);
                //order.Cost += orderItem.Price;
            }
            order.Discount = 0m;
            order.Price = order.Cost;
            if (!string.IsNullOrEmpty(order.CouponCode))
            {
                var voucher = order.CouponCode;
                if (couponService.IsCouponActive(voucher))
                {
                    var left = couponService.UseCoupon(voucher, order.Cost);

                    // either equal or something else has to be paid
                    if (left <= 0)
                    {
                        order.Price = Math.Abs(left);
                        order.Discount = order.Cost - order.Price;
                    }
                    else
                    {
                        order.Price = 0.0m;
                        order.Discount = order.Cost;
                        order.OrderStatus = (int)OrderStatus.Paid;
                    }
                }
            }

            Repository.SaveChanges();
            
            //Cusomer haven't to pay
            if (order.Price == 0) return string.Format("/{0}/singleletter/confirmation", orderContext.CurrentCulture);

            return paymentService.Process(orderContext);
        }

        private string GenerateOrderCode()
        {
            string orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();

            if (Repository.DbOrders.Any(c => c.OrderCode == orderCode))
            {
                orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();
            }

            return orderCode;
        }

        public void MarkOrderIsPaid(int orderId)
        {
            DbOrders order = Repository.DbOrders.FirstOrDefault(c => c.Id == orderId);
            if (order.OrderStatus == (int)OrderStatus.Created)
            {
                order.OrderStatus = (int)OrderStatus.Paid;
                order.DatePaid = DateTime.Now;
                Repository.SaveChanges();
            }
        }

        public void MarkOrderIsDone(int orderId)
        {
            DbOrders order = Repository.DbOrders.FirstOrDefault(c => c.Id == orderId);
            if (order.OrderStatus == (int)OrderStatus.Paid)
            {
                order.OrderStatus = (int)OrderStatus.Paid;
                order.DatePaid = DateTime.Now;
                Repository.SaveChanges();
            }
        }

        public PaginatedResult<Order> GetOrdersShouldBeDelivered(PaginatedCriteria criteria)
        {
            var dbOrder = Repository.DbOrders.Where(c => c.OrderStatus == (int) OrderStatus.Paid &&
                                                         c.OrderType == (int) OrderType.SendLetter).OrderBy(c=>c.DateCreated).Skip(criteria.PageIndex*criteria.PageSize).Take(criteria.PageSize);

            var orders = orderFactory.Create(dbOrder.ToList());

            PaginatedResult<Order> pagResult = new PaginatedResult<Order>();
            foreach (var order in orders)
            {
                pagResult.Results.Add(order);
            }
            return pagResult;
        }

        public void MarkLetterIsSent(int letterId)
        {
            var dbletter = Repository.DbLetters.FirstOrDefault(c => c.Id == letterId);

            if (dbletter == null)
            {
                throw new ArgumentException("Letter does not exist");
            }


            if (dbletter.LetterStatus == (int)LetterStatus.Created)
            {
                dbletter.LetterStatus = (int)LetterStatus.Sent;
                Repository.SaveChanges();
            }
        }

        public void MarkOrdersIsDone(IList<Order> orders)
        {
            foreach (var order in orders)
            {
                var dbOrder = Repository.DbOrders.FirstOrDefault(c => c.Id == order.Id);

                if (dbOrder == null)
                {
                    break;
                }

                foreach (var item in dbOrder.DbOrderItems)
                {
                    item.DbLetters.LetterStatus = (int) LetterStatus.Sent;
                }
                order.OrderStatus = OrderStatus.Done;
            }
            Repository.SaveChanges();
        }

        public PaginatedResult<Order> GetOrders(OrderCriteria criteria)
        {
            return new PaginatedResult<Order>();
            //System.Linq.Expressions.Expression<Func<Order, bool>> query = PredicateBuilder.True<Order>();
            //if (criteria.CustomerId > 0)
            //{
            //    query = query.And(o => o.CustomerId == criteria.CustomerId);
            //}
            //if (criteria.From.HasValue)
            //{
            //    query = query.And(o => o.DateCreated >= criteria.From);
            //}
            //if (criteria.To.HasValue)
            //{
            //    query = query.And(o => o.DateCreated <= criteria.To);
            //}
            //if (criteria.OrderType.HasValue)
            //{
            //    query = query.And(o => o.OrderType == criteria.OrderType.Value);
            //}
            //return repository.Find<Order>(query, criteria.PageIndex, criteria.PageSize, criteria.OrderBy.ToArray());
        }

        public string AddFunds(int customerId, decimal price)
        {
            var dbcustomer = Repository.DbCustomers.FirstOrDefault(c => c.Id == customerId);

            if (dbcustomer == null)
            {
                throw new ArgumentException("Customer doesnt exist");
            }

            var order = new DbOrders();
            order.CustomerId = dbcustomer.Id;
            order.DbCustomers = dbcustomer;
            order.Email = dbcustomer.Email;
            order.OrderType = (int)OrderType.SendLetter;
            order.Guid = Guid.NewGuid().ToString();
            order.OrderCode = GenerateOrderCode();
            order.OrderStatus =(int)OrderStatus.Created;
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.Cost = price;
            order.PaymentMethod = PaypalMethod.NAME;


            DbOrderItems orderItem = new DbOrderItems();
            orderItem.Price = price;
            orderItem.DbOrders = order;
            orderItem.OrderId = order.Id;

            order.DbOrderItems = new List<DbOrderItems>();
            order.DbOrderItems.Add(orderItem);

            order.Discount = 0m;
            order.Price = order.Cost;

            Repository.DbOrders.Add(order);
            Repository.SaveChanges();

            var realOrder = GetOrderById(order.Id);
            var realCustomer = customerService.GetCustomerById(dbcustomer.Id);

            return paymentService.Process(new OrderContext() { Order = realOrder, Customer = realCustomer, CurrentCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName });
        }

        public void AddFundsForAccount(int orderId)
        {
            DbOrders dborder = Repository.DbOrders.FirstOrDefault(c => c.Id == orderId);
            if (dborder.OrderStatus != (int)OrderStatus.Paid)
            {
                throw new BusinessException("The order is not paid!");
            }
            var customer = Repository.DbCustomers.FirstOrDefault(c => c.Id == dborder.CustomerId);
            customer.DateUpdated = DateTime.Now;
            customer.Credits += dborder.Price;
            Repository.SaveChanges();
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

        public void DeleteOrder(int orderId)
        {
            var order = Repository.DbOrders.FirstOrDefault(c => c.Id == orderId);
            if (order == null) return;
            order.OrderStatus = (int)OrderStatus.Cancelled;
            Repository.SaveChanges();
        }
    }
}
