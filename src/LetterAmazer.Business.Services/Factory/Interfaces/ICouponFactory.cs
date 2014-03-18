using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface ICouponFactory
    {
        Coupon Create(DbCoupons dbCoupons);
        List<Coupon> Create(List<DbCoupons> dbCouponses);
    }
}