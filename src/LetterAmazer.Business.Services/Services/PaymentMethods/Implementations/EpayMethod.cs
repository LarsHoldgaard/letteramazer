using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor.Installer;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class EpayMethod:IPaymentMethod 
    {
        private string payUrl { get; set; }

        public EpayMethod()
        {
            this.payUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Epay.PayUrl");


        }

        public string Process(Order order)
        {
            string paymentUrl = string.Format(payUrl, order.Id);
            return paymentUrl;
        }

        public void VerifyPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public void CallbackNotification()
        {
            throw new NotImplementedException();
        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
