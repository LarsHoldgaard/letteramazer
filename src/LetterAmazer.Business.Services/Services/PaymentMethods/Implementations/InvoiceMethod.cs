using System;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class InvoiceMethod : IPaymentMethod
    {
        public void Process(Order order)
        {
            throw new BusinessException("Cannot purchase orders with invoice");
        }

        public void ProcessFunds(Customer customer, decimal value)
        {
            throw new NotImplementedException();
        }
    }
}
