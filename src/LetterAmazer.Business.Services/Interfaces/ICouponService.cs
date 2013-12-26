using LetterAmazer.Business.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
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
