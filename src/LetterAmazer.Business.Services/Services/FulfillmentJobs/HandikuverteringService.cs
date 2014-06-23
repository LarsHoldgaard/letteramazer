using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.Orders;
using log4net;

namespace LetterAmazer.Business.Services.Services.FulfillmentJobs
{
    public class HandikuverteringService : IFulfillmentService
    {
        // this is 20mb
        private const int thresholdBytes = 20971520;

        private static readonly ILog logger = LogManager.GetLogger(typeof(IntermailService));

        private IMailService mailService;
        private IOrderService orderService;
        private bool isactivated;
        private string email;

        public HandikuverteringService(IOrderService orderService, IMailService mailService)
        {
            this.isactivated = bool.Parse(ConfigurationManager.AppSettings.Get("LetterAmazer.Settings.SendLetters"));

            this.mailService = mailService;
            this.orderService = orderService;

            this.email = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Handikuvertering.Email"];
        }



        public void Process(IEnumerable<Letter> letters)
        {
            int currentFileSize = 0;
            List<Letter> mailsInThisLetter = new List<Letter>();
            foreach (var letter in letters)
            {
                currentFileSize += letter.LetterContent.Content.Length;

                // time to create a new email
                if (currentFileSize > thresholdBytes)
                {
                    SendLetters(mailsInThisLetter);
                    mailsInThisLetter.Clear();
                    currentFileSize = letter.LetterContent.Content.Length;
                }
                mailsInThisLetter.Add(letter);    
                
            }
            SendLetters(mailsInThisLetter);

            orderService.UpdateByLetters(letters);
            
        }

        private void SendLetters(List<Letter> letters)
        {
            mailService.SendHandimailFulfillment(email,letters);

        }
    }

}
