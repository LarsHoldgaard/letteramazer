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
            order.OrderStatus = OrderStatus.Created;
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.Cost = letterService.GetCost(order.Letters.Count, order.Letters.ElementAt(0).ToAddress);
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
                    }
                }
            }
            repository.Create(order);
            unitOfWork.Commit();
            return paymentService.Process(orderContext);
        }
    }
}
