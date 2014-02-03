using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Data.Repository.Data;
using LinqKit;
using System.Threading;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services
{
    public class OrderService : IOrderService
    {
        private LetterAmazerEntities Repository;
        private ILetterService letterService;
        private IPaymentService paymentService;
        private ICouponService couponService;

        public OrderService(LetterAmazerEntities repository,
            ILetterService letterService, IPaymentService paymentService,
            ICouponService couponService)
        {
            this.Repository = repository;
            this.letterService = letterService;
            this.paymentService = paymentService;
            this.couponService = couponService;
        }

        public string CreateOrder(OrderContext orderContext)
        {
            DbOrder order = orderContext.Order;
            order.OrderType = OrderType.SendLetter;
            order.Guid = Guid.NewGuid();
            order.OrderCode = GenerateOrderCode();
            order.OrderStatus = OrderStatus.Created;
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.Cost = 0m;
            foreach (var orderItem in order.OrderItems)
            {
                repository.Create(orderItem.Letter);
                orderItem.Price = letterService.GetCost(orderItem.Letter);
                order.Cost += orderItem.Price;
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
                        order.OrderStatus = OrderStatus.Paid;
                    }
                }
            }
            repository.Create(order);
            unitOfWork.Commit();
            
            //Cusomer haven't to pay
            if (order.Price == 0) return string.Format("/{0}/singleletter/confirmation", orderContext.CurrentCulture);

            return paymentService.Process(orderContext);
        }

        private string GenerateOrderCode()
        {
            string orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();
            while (repository.Exists<Order>(o => o.OrderCode == orderCode))
            {
                orderCode = "LA" + DateTime.Now.Ticks.GetHashCode();
            }
            return orderCode;
        }

        public void MarkOrderIsPaid(int orderId)
        {
            Order order = repository.GetById<Order>(orderId);
            if (order.OrderStatus == OrderStatus.Created)
            {
                order.OrderStatus = OrderStatus.Paid;
                order.DatePaid = DateTime.Now;
                unitOfWork.Commit();
            }
        }

        public void MarkOrderIsDone(int orderId)
        {
            Order order = repository.GetById<Order>(orderId);
            if (order.OrderStatus == OrderStatus.Paid)
            {
                order.OrderStatus = OrderStatus.Done;
                order.DateSent = DateTime.Now;
                unitOfWork.Commit();
            }
        }

        public PaginatedResult<Order> GetOrdersShouldBeDelivered(PaginatedCriteria criteria)
        {
            return repository.Find<Order>(o => o.OrderStatus == OrderStatus.Paid && o.OrderType == OrderType.SendLetters, criteria.PageIndex, criteria.PageSize, OrderBy.Asc("DateCreated"));
        }

        public void MarkLetterIsSent(int letterId)
        {
            Letter letter = repository.GetById<Letter>(letterId);
            if (letter.LetterStatus == LetterStatus.Created)
            {
                letter.LetterStatus = LetterStatus.Sent;
                unitOfWork.Commit();
            }
        }

        public void MarkOrdersIsDone(IList<Order> orders)
        {
            foreach (var order in orders)
            {
                foreach (var item in order.OrderItems)
                {
                    item.Letter.LetterStatus = LetterStatus.Sent;
                }
                order.OrderStatus = OrderStatus.Done;
            }
            unitOfWork.Commit();
        }

        public PaginatedResult<Order> GetOrders(OrderCriteria criteria)
        {
            System.Linq.Expressions.Expression<Func<Order, bool>> query = PredicateBuilder.True<Order>();
            if (criteria.CustomerId > 0)
            {
                query = query.And(o => o.CustomerId == criteria.CustomerId);
            }
            if (criteria.From.HasValue)
            {
                query = query.And(o => o.DateCreated >= criteria.From);
            }
            if (criteria.To.HasValue)
            {
                query = query.And(o => o.DateCreated <= criteria.To);
            }
            if (criteria.OrderType.HasValue)
            {
                query = query.And(o => o.OrderType == criteria.OrderType.Value);
            }
            return repository.Find<Order>(query, criteria.PageIndex, criteria.PageSize, criteria.OrderBy.ToArray());
        }

        public string AddFunds(int customerId, decimal price)
        {
            Customer customer = repository.GetById<Customer>(customerId);
            Order order = new Order();
            order.CustomerId = customerId;
            order.Customer = customer;
            order.Email = customer.Email;
            order.OrderType = OrderType.AddFunds;
            order.Guid = Guid.NewGuid().ToString();
            order.OrderCode = GenerateOrderCode();
            order.OrderStatus = OrderStatus.Created;
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.Cost = price;
            order.PaymentMethod = LetterAmazer.Business.Services.Services.PaymentMethod.PaypalMethod.NAME;
            OrderItem orderItem = new OrderItem();
            orderItem.Price = price;
            orderItem.Order = order;

            order.OrderItems = new List<OrderItem>();
            order.OrderItems.Add(orderItem);

            order.Discount = 0m;
            order.Price = order.Cost;
            
            repository.Create(order);
            unitOfWork.Commit();

            return paymentService.Process(new OrderContext() { Order = order, Customer = customer, CurrentCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName });
        }

        public void AddFundsForAccount(int orderId)
        {
            Order order = repository.GetById<Order>(orderId);
            if (order.OrderStatus != OrderStatus.Paid)
            {
                throw new BusinessException("The order is not paid!");
            }
            Customer customer = repository.GetById<Customer>(order.CustomerId.Value);
            customer.DateUpdated = DateTime.Now;
            customer.Credits += order.Price;
            repository.Update(customer);
        }

        public Order GetOrderById(int orderId)
        {
            Order order = repository.GetById<Order>(orderId);
            if (order == null)
            {
                throw new ItemNotFoundException("Order");
            }
            return order;
        }

        public void DeleteOrder(int orderId)
        {
            Order order = repository.GetById<Order>(orderId);
            if (order == null) return;
            order.OrderStatus = OrderStatus.Cancelled;
            repository.Update(order);
        }
    }
}
