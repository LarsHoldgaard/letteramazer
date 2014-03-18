using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public interface IPaymentMethod
    {
        string Process(Order order);
        void VerifyPayment(Order order);
        void ChargeBacks(Order order);
    }
}
