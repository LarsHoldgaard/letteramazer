using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Utils.Helpers;
using System;
using System.Linq;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CouponService : ICouponService
    {
        private const int CouponLength = 7;
        private LetterAmazerEntities Repository;
        private CouponFactory CouponFactory;

        public CouponService(LetterAmazerEntities repository, CouponFactory couponFactory)
        {
            Repository = repository;
            CouponFactory = couponFactory;
        }


        public string GenerateCoupon(Coupon coupon)
        {
            var code = PasswordGenerator.Generate(CouponLength);

            var dbcoupon = new DbCoupons(){
                Code = code,
                CouponExpire = DateTime.Now.AddDays(7), //TODO: Fix :D
                RefSource = coupon.RefSource,
                RefUserValue = coupon.RefUserValue,
                CouponValue = coupon.CouponValue,
                DateGiven = DateTime.Now,
                EarlierCouponRef = coupon.EarlierCouponRef,
                CouponValueLeft = coupon.CouponValueLeft,
                CouponStatus = (int)coupon.CouponStatus
            };

            Repository.DbCoupons.Add(dbcoupon);
            Repository.SaveChanges();

            return code;

        }

        public bool IsCouponActive(int id)
        {
            var anyCode = Repository.DbCoupons.Any(c => c.Id == id && c.CouponExpire > DateTime.Now && c.CouponStatus != (int)CouponStatus.Done && c.CouponValueLeft > 0.0m);
            return anyCode;
        }


        public bool IsCouponActive(string code)
        {
            var lowerCode = code.ToLower();
            var anyCode = Repository.DbCoupons.Any(c => c.Code == lowerCode && c.CouponExpire > DateTime.Now && c.CouponStatus != (int)CouponStatus.Done && c.CouponValueLeft > 0.0m);
            return anyCode;
        }

        public Coupon GetCouponByCode(string code)
        {
            code = code.ToLower();
            if (!IsCouponActive(code))
            {
                throw new ArgumentException("Code has been used");
            }

            var currentCode = Repository.DbCoupons.FirstOrDefault(c => c.Code == code);
            if (currentCode == null)
            {
                throw new ItemNotFoundException("Coupon");
            }

            var coupon = CouponFactory.Create(currentCode);

            return coupon;
        }

        public Coupon GetCouponById(int id)
        {
            if (!IsCouponActive(id))
            {
                throw new ArgumentException("Code has been used");
            }

            var currentCode = Repository.DbCoupons.FirstOrDefault(c => c.Id == id);
            if (currentCode == null)
            {
                throw new ItemNotFoundException("Coupon");
            }

            var coupon = CouponFactory.Create(currentCode);

            return coupon;
        }

        public decimal UseCoupon(string code, decimal price)
        {
            code = code.ToLower();
            if (!IsCouponActive(code))
            {
                throw new BusinessException("Code has been used");
            }

            var currentCode = Repository.DbCoupons.FirstOrDefault(c => c.Code == code);

            if (currentCode == null)
            {
                throw new ItemNotFoundException("Coupon");
            }

            decimal differenceInValue = currentCode.CouponValueLeft- price;

            // credits left
            if (differenceInValue > 0)
            {
                currentCode.CouponValueLeft = differenceInValue;
                currentCode.CouponStatus = (int)CouponStatus.Used;
            }
            else
            {
                currentCode.CouponStatus = (int)CouponStatus.Done;
                currentCode.CouponValueLeft = 0;
            }

            Repository.SaveChanges();

            return differenceInValue;
        }
    }
}
