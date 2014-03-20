﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.IdentityManagement.Model;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using log4net;

namespace LetterAmazer.Business.Services.Services.FulfillmentJobs
{
    public class IntermailService: IFulfillmentService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(IntermailService));

        private ILetterService letterService;
        private IOrderService orderService;
        private bool isactivated;

        public string FtpServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public IntermailService(ILetterService letterService, IOrderService orderService)
        {
            this.isactivated = bool.Parse(ConfigurationManager.AppSettings.Get("LetterAmazer.Settings.SendLetters"));

            this.letterService = letterService;
            this.orderService = orderService;

            this.FtpServer = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Intermail.FtpServer"];
            this.Username = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Intermail.Username"];
            this.Password = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Intermail.Password"];
        }

        public void Process(IEnumerable<Letter> letters)
        {
            foreach (var letter in letters)
            {
                try
                {
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(FtpServer);
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
                catch (Exception ex)
                {
                    logger.Fatal("Intermail job error in delivery: " + ex + " || " + ex.InnerException);
                }
            }

            orderService.UpdateByLetters(letters);

        }
    }
}
