namespace LetterAmazer.Business.Services.Domain.Coupons
{
    public interface ICouponService
    {
        bool IsCouponActive(string code);
        bool IsCouponActive(int id);

        string GenerateCoupon(Coupon coupon);
        Coupon GetCouponByCode(string code);
        Coupon GetCouponById(int id);

        decimal UseCoupon(string code, decimal price);
    }
}
