using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public interface IPaymentService
    {
        string Process(Order order);
        List<PaymentMethods> GetPaymentMethodsBySpecification(PaymentMethodSpecification specification);

        PaymentMethods GetPaymentMethodById(int id);
    }
}
