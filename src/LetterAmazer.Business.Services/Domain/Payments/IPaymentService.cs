using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public interface IPaymentService
    {
        void Process(PaymentMethods method, Order order);
        void ProcessFunds(PaymentMethods method, Customer customer, decimal value);
        List<PaymentMethods> GetPaymentMethodsBySpecification(PaymentMethodSpecification specification);
    }
}
