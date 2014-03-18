using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Services.FulfillmentJobs
{
    public class IntermailService: IFulfillmentService
    {
        private ILetterService letterService;
        private IOrderService orderService;
        private bool isactivated;

        public IntermailService(ILetterService letterService, IOrderService orderService)
        {
            this.isactivated = bool.Parse(ConfigurationManager.AppSettings.Get("LetterAmazer.Settings.SendLetters"));

            this.letterService = letterService;
            this.orderService = orderService;
        }

        public void Process(IEnumerable<Letter> letters)
        {
            foreach (var letter in letters)
            {
                
            }
        }
    }
}
