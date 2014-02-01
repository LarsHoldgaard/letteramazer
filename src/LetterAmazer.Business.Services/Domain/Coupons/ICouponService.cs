using LetterAmazer.Business.Services.Data;

namespace LetterAmazer.Business.Services.Domain.Coupons
{
    public interface ICouponService
    {
        bool IsCouponActive(string code);
        bool VerifyAminoUser(string username);
        string GenerateCoupon(decimal value, int daysToExpire, string source, string sourcevalue);
        string GenerateCoupon(decimal value, int daysToExpire, string source, string sourcevalue, int earlierCoupon);
        Coupon GetCoupon(string code);
        decimal UseCoupon(string code, decimal price);
    }
}
