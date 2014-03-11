using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using log4net;

namespace LetterAmazer.Business.Services.Services.FulfillmentJobs
{
    public class PostalMethodsService : IFulfillmentService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PostalMethodsService));
        private string username;
        private string password;
        private ILetterService letterService;
        private IOrderService orderService;

        public PostalMethodsService(ILetterService letterService, IOrderService orderService)
        {
            this.letterService = letterService;
            this.orderService = orderService;
        }

        public void Process(IEnumerable<Letter> letters)
        {
            const com.postalmethods.api.WorkMode workMode = com.postalmethods.api.WorkMode.Development;

            foreach (var letter in letters)
            {
                var letterId = "LA_" + letter.Id;
                com.postalmethods.api.PostalWS objPM = new com.postalmethods.api.PostalWS();

                long value = objPM.SendLetter(username, password, letterId, "pdf", letter.LetterContent.Content, workMode);

                if (value < 0)
                {
                    logger.Error("Error postal methods with letterid " + letter.Id);
                }

            }
        }
    }
}
