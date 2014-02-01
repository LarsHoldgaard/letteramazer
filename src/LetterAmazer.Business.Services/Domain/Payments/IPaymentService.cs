using LetterAmazer.Business.Services.Model;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public interface IPaymentService
    {
        string Process(OrderContext orderContext);
        IPaymentMethod Get(string methodName);
    }
}
