using System;
using System.Collections.Generic;
using System.Configuration;
using iTextSharp.text;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Utils.Helpers;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class InvoiceMethod : IPaymentMethod
    {
        private IInvoiceService invoiceService;
        private IOrderService orderService;
        private ICountryService countryService;
        private string baseUrl;
        private string serviceUrl;

        public InvoiceMethod(IInvoiceService invoiceService, IOrderService orderService, ICountryService countryService)
        {
            this.baseUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.BasePath");
            this.serviceUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Invoice.ServiceUrl");
            this.orderService = orderService;
            this.invoiceService = invoiceService;
            this.countryService = countryService;
        }

        public string Process(Order order)
        {
            var invoiceCountry = countryService.GetCountryById(Constants.Texts.PracticalInformation.CountryId);

            var invoice = new Invoice()
            {
                DateDue = DateTime.Now.AddDays(14),
                PriceExVat = order.Price.PriceExVat,
                InvoiceLines = getInvoiceLines(order.OrderLines),
                PriceTotal = order.Price.Total,
                PriceVatPercentage = order.Price.VatPercentage,
                InvoiceInfo = new AddressInfo()
                {
                    Address1 = Constants.Texts.PracticalInformation.Address1,
                    Zipcode = Constants.Texts.PracticalInformation.Zipcode,
                    Country = invoiceCountry,
                    VatNr = Constants.Texts.PracticalInformation.VatNumber,
                    Organisation = Constants.Texts.PracticalInformation.CompanyName,
                   AttPerson= Constants.Texts.PracticalInformation.AttPerson
                },
                ReceiverInfo = new AddressInfo()
                {
                    Address1 = order.Customer.CustomerInfo.Address1,
                    Address2 = order.Customer.CustomerInfo.Address2,
                    Zipcode = order.Customer.CustomerInfo.Zipcode,
                    City = order.Customer.CustomerInfo.City,
                    State = order.Customer.CustomerInfo.State,
                    Country = order.Customer.CustomerInfo.Country,
                    AttPerson = order.Customer.CustomerInfo.AttPerson,
                    Organisation = order.Customer.CustomerInfo.Organisation,
                    VatNr = order.Customer.CustomerInfo.VatNr,
                    
                },
                OrganisationId = order.Customer.OrganisationId,
                Guid = Guid.NewGuid(),
                OrderId = order.Id
            };
            var stored_invoice = invoiceService.Create(invoice);

            var url = baseUrl + serviceUrl;
            var fullurl = string.Format(url, stored_invoice.Guid);
            return fullurl;
        }

        public void VerifyPayment(Order order)
        {
            order.OrderStatus = OrderStatus.Paid;
            orderService.ReplenishOrderLines(order);
            orderService.Update(order);
        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }

        #region Private helpers

        private List<InvoiceLine> getInvoiceLines(List<OrderLine> orderLines)
        {
            List<InvoiceLine> invoiceLines = new List<InvoiceLine>();
            foreach (var orderLine in orderLines)
            {
                invoiceLines.Add(new InvoiceLine()
                {
                    Description = orderLine.ProductType.ToString(),
                    Quantity =orderLine.Quantity,
                    PriceExVat = orderLine.Price.PriceExVat
                });
            }

            return invoiceLines;
        }

        #endregion
    }
}
