using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Services.FulfillmentJobs
{
    public class PostalMethodsService : IFulfillmentService
    {

        private ILetterService letterService;
        private IOrderService orderService;

        public PostalMethodsService(ILetterService letterService, IOrderService orderService)
        {
            this.letterService = letterService;
            this.orderService = orderService;
        }

        public void Process(IEnumerable<Letter> letters)
        {
            foreach (var letter in letters)
            {
                // use postal methods API to send some letters
            }
        }
    }
}
