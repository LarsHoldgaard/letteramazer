using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services
{
    public class OrderService : IOrderService
    {
        private IRepository repository;
        private IUnitOfWork unitOfWork;
        private ILetterService letterService;
        private IPaymentService paymentService;
        private ICouponService couponService;

        public OrderService(IRepository repository, IUnitOfWork unitOfWork, 
            ILetterService letterService, IPaymentService paymentService,
            ICouponService couponService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.letterService = letterService;
            this.paymentService = paymentService;
            this.couponService = couponService;
        }

        public string CreateOrder(OrderContext orderContext)
        {
            Order order = orderContext.Order;
            order.Guid = Guid.NewGuid().ToString();
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
            if (order.Price == 0) return "/singleletter/confirmation";

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
                unitOfWork.Commit();
            }
        }
    }
}
