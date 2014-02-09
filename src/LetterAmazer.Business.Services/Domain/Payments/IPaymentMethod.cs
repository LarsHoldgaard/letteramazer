using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public interface IPaymentMethod
    {
        void Process(IPurchasable purchasable);
        void VerifyPayment(IPurchasable purchasable);
        void ChargeBacks(IPurchasable purchasable);

        //string Name { get; }
        //string Process(OrderContext orderContext);
        //VerifyPaymentResult Verify(VerifyPaymentContext context);
    }
}
