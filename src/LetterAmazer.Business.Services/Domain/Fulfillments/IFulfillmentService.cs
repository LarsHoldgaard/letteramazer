using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Fulfillments
{
    public interface IFulfillmentService
    {
        void Process(IEnumerable<Letter> letters);
    }
}
