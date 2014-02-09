using System;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class InvoiceMethod : IPaymentMethod
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
