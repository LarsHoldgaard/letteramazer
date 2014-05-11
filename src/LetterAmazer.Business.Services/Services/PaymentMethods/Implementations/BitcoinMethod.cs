using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class BitcoinMethod : IPaymentMethod
    {
        public string Process(Order order)
        {
            throw new NotImplementedException();
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
