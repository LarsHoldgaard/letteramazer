using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.com.postalmethods.api;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Factory;
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
        private bool isactivated;

        public PostalMethodsService(ILetterService letterService, IOrderService orderService)
        {
            this.isactivated = bool.Parse(ConfigurationManager.AppSettings.Get("LetterAmazer.Settings.SendLetters"));

            this.username = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.PostalMethods.Username"];
            this.password = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.PostalMethods.Password"];

            this.letterService = letterService;
            this.orderService = orderService;
        }

        public void Process(IEnumerable<Letter> letters)
        {
            WorkMode workMode;
            if (isactivated)
            {
                workMode =WorkMode.Production;
            }
            else
            {
                workMode = WorkMode.Development;
            }

            foreach (var letter in letters)
            {
                var letterId = "LA_" + letter.Id;
                PostalWS objPM = new PostalWS();

                long value = objPM.SendLetter(username, password, letterId, "pdf", letter.LetterContent.Content, workMode);

                if (value < 0)
                {
                    logger.Error("Error postal methods with letterid " + letter.Id);
                }

            }

            orderService.UpdateByLetters(letters);
        }
    }
}
