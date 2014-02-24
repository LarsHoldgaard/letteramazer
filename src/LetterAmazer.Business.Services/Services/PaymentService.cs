﻿using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Services.PaymentMethods.Implementations;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private IPaymentFactory paymentFactory;
        private LetterAmazerEntities repository;
        private IPriceService priceService;
        private ICouponService couponService;
        public PaymentService(LetterAmazerEntities repository,IPriceService priceService,IPaymentFactory paymentFactory, ICouponService couponService)
        {
            this.priceService = priceService;
            this.repository = repository;
            this.paymentFactory = paymentFactory;
            this.couponService = couponService;
        }

        public string Process(Order order)
        {
            var paymentMethods = order.OrderLines.Where(c => c.ProductType == ProductType.Payment);
            var url = string.Empty;

            foreach (var paymentMethod in paymentMethods)
            {
                if (paymentMethod.PaymentMethod != null && !string.IsNullOrEmpty(paymentMethod.PaymentMethod.Name))
                {
                    var pm = getPaymentMethod(paymentMethod.PaymentMethod.Name);
                    url = pm.Process(order);
                }    
            }

            return url;
        }

        public List<Domain.Payments.PaymentMethods> GetPaymentMethodsBySpecification(PaymentMethodSpecification specification)
        {
            IQueryable<DbPaymentMethods> dbPaymentMethods = repository.DbPaymentMethods;
            dbPaymentMethods = dbPaymentMethods.Where(c => c.IsVisible && (c.DateDeleted == null||c.DateDeleted > DateTime.Now));

            // Credits always useable for customers with credits
            if (specification.CustomerId == 0)
            {   
                dbPaymentMethods = dbPaymentMethods.Where(c => !c.RequiresLogin);
            }
            if (specification.TotalPrice > 0) 
            {
                dbPaymentMethods =
                    dbPaymentMethods.Where(
                        c => c.MinimumAmount <= specification.TotalPrice && c.MaximumAmount >= specification.TotalPrice);
            }

            return paymentFactory.Create(dbPaymentMethods.OrderBy(c=>c.SortOrder).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public Domain.Payments.PaymentMethods GetPaymentMethodById(int id)
        {
            DbPaymentMethods dbPayment = repository.DbPaymentMethods.FirstOrDefault(c => c.Id == id);
            if (dbPayment == null)
            {
                throw new ItemNotFoundException("Payment method doesn't exist");
            }

            var paymentMethod = paymentFactory.Create(dbPayment);
            return paymentMethod;
        }

        #region Private methods

        private IPaymentMethod getPaymentMethod(string name)
        {
            if (name == "Credit")
            {
                return new CreditsMethod();
            }
            else if (name == "Coupon")
            {
                return new CouponMethod(couponService);
            }
            else if (name == "Invoice")
            {
                return new InvoiceMethod();
            }
            else if (name == "Bitcoin")
            {
                return new BitcoinMethod();
            }
            else
            {
                return new PaypalMethod(priceService);
            }
        }

        #endregion
    }
}
