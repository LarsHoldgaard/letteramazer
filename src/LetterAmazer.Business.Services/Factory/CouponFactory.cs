using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class CouponFactory
    {
        public Coupon Create(DbCoupons dbCoupons)
        {
            return new Coupon()
            {
                Code = dbCoupons.Code,
                Id = dbCoupons.Id,
                DateModified = dbCoupons.DateModified,
                DateCreated = dbCoupons.DateGiven,
                CouponStatus = (CouponStatus)dbCoupons.CouponStatus,
                CouponValue = dbCoupons.CouponValue,
                CouponValueLeft = dbCoupons.CouponValueLeft,
                RefSource = dbCoupons.RefSource,
                RefUserValue = dbCoupons.RefUserValue,
                EarlierCouponRef = dbCoupons.EarlierCouponRef.HasValue ? dbCoupons.EarlierCouponRef.Value: 0,
                ExpireDate = dbCoupons.CouponExpire,
                
            };


        }
    }
}
