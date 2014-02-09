using System.Collections.Generic;
using iTextSharp.text;

namespace LetterAmazer.Business.Services.Domain.Coupons
{
    public interface ICouponService
    {
        Coupon Update(Coupon coupon);
        Coupon Create(Coupon coupon);
        void Delete(Coupon coupon);

        Coupon GetCouponById(int id);
        List<Coupon> GetCouponBySpecification(CouponSpecification specification);

    }
}
