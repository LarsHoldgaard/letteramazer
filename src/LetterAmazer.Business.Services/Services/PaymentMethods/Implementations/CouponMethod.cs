using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class CouponMethod : IPaymentMethod
    {
        private ICouponService couponService;

        public CouponMethod(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        public string Process(Order order)
        {
            var orderLine = order.OrderLines.FirstOrDefault(c => c.ProductType == ProductType.Payment && c.PaymentMethod.Id == 3);

            if (orderLine == null)
            {
                throw new BusinessException("Coupon line wasn't found on order");
            }

            var coupon = couponService.GetCouponById(orderLine.CouponId);

            coupon.CouponValueLeft -= orderLine.Cost;

            if (coupon.CouponValueLeft > 0)
            {
                coupon.CouponStatus=CouponStatus.Used;
            }
            else
            {
                coupon.CouponStatus = CouponStatus.Done;
            }

            couponService.Update(coupon);

            return string.Empty;
        }

        public void VerifyPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
