using LetterAmazer.Business.Services.Model;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public interface IPaymentMethod
    {
        string Name { get; }
        string Process(OrderContext orderContext);
        VerifyPaymentResult Verify(VerifyPaymentContext context);
    }
}
