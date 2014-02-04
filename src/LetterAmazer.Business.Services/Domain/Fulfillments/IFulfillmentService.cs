using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Fulfillments
{
    public interface IFulfillmentService
    {
        void Process(IList<Order> orders);
    }
}
