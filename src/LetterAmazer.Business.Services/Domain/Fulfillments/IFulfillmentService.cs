using System.Collections.Generic;
using LetterAmazer.Business.Services.Data;

namespace LetterAmazer.Business.Services.Domain.Fulfillments
{
    public interface IFulfillmentService
    {
        void Process(IList<Order> orders);
    }
}
