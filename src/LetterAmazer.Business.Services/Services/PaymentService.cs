using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Services.PaymentMethods.Implementations;

namespace LetterAmazer.Business.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private ICustomerService customerService;
        private IPriceService priceService;
        private ICouponService couponService;

        public PaymentService(IPriceService priceService, ICustomerService customerService, ICouponService couponService)
        {
            this.priceService = priceService;
            this.customerService = customerService;
            this.couponService = couponService;
        }

        public string Process(List<Domain.Payments.PaymentMethods> methods, Order order)
        {
            var url = string.Empty;
            var usedMethods = getPaymentMethods(methods);

            foreach (var method in usedMethods)
            {
                url = method.Process(order);
            }

            return url;
        }

       

        public List<Domain.Payments.PaymentMethods> GetPaymentMethodsBySpecification(PaymentMethodSpecification specification)
        {
            var paymentMethods = new List<Domain.Payments.PaymentMethods>();

            // Credits alway useable for customers with credits
            if (specification.CustomerId > 0)
            {
                var customer = customerService.GetCustomerById(specification.CustomerId);
                if (customer.Credit > customer.CreditLimit)
                {
                    paymentMethods.Add(Domain.Payments.PaymentMethods.Credits);
                }
            }

            if (specification.OrderType != null)
            {
                if (specification.OrderType.Value == OrderType.Credits)
                {
                    paymentMethods.Add(Domain.Payments.PaymentMethods.Invoice);   
                }
            }

            // These payment methods are always useable
            paymentMethods.Add(Domain.Payments.PaymentMethods.Bitcoin);
            paymentMethods.Add(Domain.Payments.PaymentMethods.PayPal);
            paymentMethods.Add(Domain.Payments.PaymentMethods.Coupon);


            return paymentMethods;
        }


        #region Private helpers

        /// <summary>
        /// List of payment methods being added. SORTING IS IMPORTANT
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        private IEnumerable<IPaymentMethod> getPaymentMethods(List<Domain.Payments.PaymentMethods> methods)
        {
            List<IPaymentMethod> selectedPaymentMethods = new List<IPaymentMethod>();

            if (methods.Contains(Domain.Payments.PaymentMethods.Credits))
            {
                selectedPaymentMethods.Add(new CreditsMethod());
            }
            if (methods.Contains(Domain.Payments.PaymentMethods.Coupon))
            {
                selectedPaymentMethods.Add(new CouponMethod());
            }
            if (methods.Contains(Domain.Payments.PaymentMethods.Bitcoin))
            {
                selectedPaymentMethods.Add(new BitcoinMethod());
            }
            if (methods.Contains(Domain.Payments.PaymentMethods.PayPal))
            {
                selectedPaymentMethods.Add(new PaypalMethod(priceService));
            }
            if (methods.Contains(Domain.Payments.PaymentMethods.Invoice))
            {
                selectedPaymentMethods.Add(new InvoiceMethod());
            }

            return selectedPaymentMethods;
        }

        #endregion
    }
}
