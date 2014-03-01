using System;
using System.Configuration;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class InvoiceMethod : IPaymentMethod
    {
        private IOrderService orderService;
        private string baseUrl;
        private string serviceUrl;

        public InvoiceMethod()
        {
            this.baseUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.BasePath");
            this.serviceUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Invoice.ServiceUrl");
            this.orderService = orderService;
        }

        public string Process(Order order)
        {
            var url = baseUrl + serviceUrl;
            var fullurl = string.Format(url, order.Guid);
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
    }
}
