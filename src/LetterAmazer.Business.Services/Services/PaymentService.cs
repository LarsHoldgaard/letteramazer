using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services.PaymentMethods.Implementations;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private IPaymentFactory paymentFactory;
        private LetterAmazerEntities repository;
        private IPriceService priceService;
        private ICustomerService customerService;
        private IOrderService orderService;
        private ICountryService countryService;
        private IInvoiceService invoiceService;
        private IMailService mailService;

        public PaymentService(LetterAmazerEntities repository,IPriceService priceService,IPaymentFactory paymentFactory,  ICustomerService customerService, 
            IOrderService orderService, ICountryService countryService, IInvoiceService invoiceService,
            IMailService mailService)
        {
            this.priceService = priceService;
            this.repository = repository;
            this.paymentFactory = paymentFactory;
            this.customerService = customerService;
            this.orderService = orderService;
            this.countryService = countryService;
            this.invoiceService = invoiceService;
            this.mailService = mailService;
        }

        public string Process(Order order)
        {
            var paymentMethods = order.OrderLines.Where(c => c.ProductType == ProductType.Payment);
            var url = string.Empty;

            foreach (var orderLine in paymentMethods)
            {
                var paymentMethod = GetPaymentMethodById(orderLine.PaymentMethodId);
                if (paymentMethod != null && !string.IsNullOrEmpty(paymentMethod.Name))
                {
                    var pm = getPaymentMethod(paymentMethod.Name);
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

            if (specification.PaymentType != null)
            {
                if (specification.PaymentType.Value == PaymentType.Credits)
                {
                    // remove credits from payment methods
                    dbPaymentMethods = dbPaymentMethods.Where(c => c.Id != 2);
                }
                if (specification.PaymentType.Value == PaymentType.Letters)
                {
                    // remove invoice from payment methods
                    dbPaymentMethods = dbPaymentMethods.Where(c => c.Id != 5);
                }

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
                return new CreditsMethod(customerService,orderService);
            }
            else if (name == "Invoice")
            {
                return new InvoiceMethod(invoiceService,orderService,countryService,mailService);
            }
            else if (name == "Bitcoin")
            {
                return new BitPayMethod();
            }
            else if (name == "Epay")
            {
                return new EpayMethod();
            }
            else
            {
                return new PaypalMethod(orderService);
            }
        }

        #endregion
    }
}
