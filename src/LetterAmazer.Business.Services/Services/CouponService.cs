using System.Collections.Generic;
using Amazon.DirectConnect.Model;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Utils.Helpers;
using System;
using System.Linq;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CouponService : ICouponService
    {
        private LetterAmazerEntities Repository;
        private ICouponFactory CouponFactory;

        public CouponService(LetterAmazerEntities repository, ICouponFactory couponFactory)
        {
            Repository = repository;
            CouponFactory = couponFactory;
        }


        public Coupon Update(Coupon coupon)
        {
            var dbcoupon = Repository.DbCoupons.FirstOrDefault(c => c.Id == coupon.Id);

            if (dbcoupon == null)
            {
                return null;
            }

            dbcoupon.DateGiven = coupon.DateCreated;
            dbcoupon.DateModified = coupon.DateModified;
            dbcoupon.Code = coupon.Code;
            dbcoupon.CouponExpire = coupon.ExpireDate;
            dbcoupon.CouponStatus = (int)coupon.CouponStatus;
            dbcoupon.CouponValueLeft = coupon.CouponValueLeft;
            dbcoupon.CouponValue = coupon.CouponValue;
            dbcoupon.EarlierCouponRef = coupon.EarlierCouponRef;
            dbcoupon.RefSource=coupon.RefSource;
            dbcoupon.RefUserValue = coupon.RefUserValue;

            Repository.SaveChanges();

            return GetCouponById(dbcoupon.Id);
        }

        public Coupon Create(Coupon coupon)
        {
            var dbcoupon = new DbCoupons();
            dbcoupon.DateGiven = coupon.DateCreated;
            dbcoupon.DateModified = coupon.DateModified;
            dbcoupon.Code = coupon.Code;
            dbcoupon.CouponExpire = coupon.ExpireDate;
            dbcoupon.CouponStatus = (int)coupon.CouponStatus;
            dbcoupon.CouponValueLeft = coupon.CouponValueLeft;
            dbcoupon.CouponValue = coupon.CouponValue;
            dbcoupon.EarlierCouponRef = coupon.EarlierCouponRef;
            dbcoupon.RefSource = coupon.RefSource;
            dbcoupon.RefUserValue = coupon.RefUserValue;

            Repository.DbCoupons.Add(dbcoupon);
            Repository.SaveChanges();

            return GetCouponById(dbcoupon.Id);
        }

        public void Delete(Coupon coupon)
        {
            var dbCoupon = Repository.DbCoupons.FirstOrDefault(c => c.Id == coupon.Id);

            Repository.DbCoupons.Remove(dbCoupon);
            Repository.SaveChanges();
        }


        public Coupon GetCouponById(int id)
        {
            var dbCoupon = Repository.DbCoupons.FirstOrDefault(c => c.Id == id);

            if (dbCoupon == null)
            {
                return null;
            }

            return CouponFactory.Create(dbCoupon);
        }

        public List<Coupon> GetCouponBySpecification(CouponSpecification specification)
        {
            IQueryable<DbCoupons> dbCouponses = Repository.DbCoupons;
           
            if (specification.Id > 0)
            {
                dbCouponses = Repository.DbCoupons.Where(c => c.Id == specification.Id);
            }
            if (!string.IsNullOrEmpty(specification.Code))
            {
                dbCouponses = dbCouponses.Where(c => c.Code == specification.Code);
            }

            return CouponFactory.Create(dbCouponses.OrderBy(c=>c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }
    }
}
