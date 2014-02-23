using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public interface IPaymentFactory
    {
        PaymentMethods Create(DbPaymentMethods dbPaymentMethods);
        List<PaymentMethods> Create(List<DbPaymentMethods> dbPaymentMethodses);
    }
}