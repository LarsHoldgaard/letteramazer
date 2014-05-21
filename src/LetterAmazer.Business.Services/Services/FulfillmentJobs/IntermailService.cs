using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using log4net;

namespace LetterAmazer.Business.Services.Services.FulfillmentJobs
{
    public class IntermailService: IFulfillmentService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(IntermailService));

        private IMailService mailService;
        private IOrderService orderService;
        private bool isactivated;

        public string FtpServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public IntermailService(IOrderService orderService, IMailService mailService)
        {
            this.isactivated = bool.Parse(ConfigurationManager.AppSettings.Get("LetterAmazer.Settings.SendLetters"));

            this.mailService = mailService;
            this.orderService = orderService;

            this.FtpServer = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Intermail.FtpServer"];
            this.Username = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Intermail.Username"];
            this.Password = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Intermail.Password"];
        }

        public void Process(IEnumerable<Letter> letters)
        {
            StringBuilder status = new StringBuilder();
            foreach (var letter in letters)
            {
                try
                {
                    string colorPath = "FARVE";
                    if (letter.LetterDetails.LetterColor == LetterColor.BlackWhite)
                    {
                        colorPath = "SORTHVID";
                    }

                    string countryPath = getCountryPath(letter.ToAddress.Country);
                    string filePath = string.Format("{0}_{1}_{2}", countryPath, letter.LetterDetails.DeliveryLabel,
                        letter.Guid.ToString());
                    string servePath = FtpServer + "/" + colorPath + "/" + filePath + ".pdf";

                    if (isactivated)
                    {
                        FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(servePath);
                        ftp.Credentials = new NetworkCredential(Username, Password);

                        ftp.KeepAlive = true;
                        ftp.UseBinary = true;
                        ftp.Method = WebRequestMethods.Ftp.UploadFile;

                        var fs = new MemoryStream(letter.LetterContent.Content);
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Close();

                        Stream ftpstream = ftp.GetRequestStream();
                        ftpstream.Write(buffer, 0, buffer.Length);
                        ftpstream.Close();

                    }
                    
                    status.AppendLine("Success on: " + letter.Guid + " <br/>");
                }
                catch (Exception ex)
                {
                    logger.Fatal("Intermail job error in delivery: " + ex + " || " + ex.InnerException);
                    status.AppendLine("Error  on: " + letter.Guid + " <br/>");
                }
            }

            orderService.UpdateByLetters(letters);
            mailService.SendIntermailStatus(status.ToString(),letters.Count());
        }

        private string getCountryPath(Country country)
        {
            if (country.Id == 59)
            {
                return "Danmark";
            }
            if (country.InsideEu)
            {
                return "Europa";
            }
            return "Verden";
        }
    }
}
