using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public interface IPaymentService
    {
        void Process(List<PaymentMethods> method, Order order);
        List<PaymentMethods> GetPaymentMethodsBySpecification(PaymentMethodSpecification specification);
    }
}
