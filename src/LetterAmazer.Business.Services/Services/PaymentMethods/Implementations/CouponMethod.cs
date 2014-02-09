using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class CouponMethod : IPaymentMethod
    {
        public string Process(Order order)
        {
            throw new NotImplementedException();
        }

        public void VerifyPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
