using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Ionic.Zip;
using LetterAmazer.Business.Services.Domain.Common;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Services.Fulfillment;
using LetterAmazer.Business.Utils.Helpers;
using log4net;

namespace LetterAmazer.Business.Services.Services.FulfillmentJobs
{
    public class JupiterService : IFulfillmentService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JupiterService));
        private string zipStoragePath;
        private string pdfStoragePath;
        private string username;
        private string password;
        private string serviceUrl;
        private ILetterService letterService;
        private IOrderService orderService;

        public JupiterService(ILetterService letterService, IOrderService orderService)
        {
            this.serviceUrl = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Jupiter.ServiceUrl"]; ;
            this.username = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Jupiter.Username"]; ;
            this.password = ConfigurationManager.AppSettings["LetterAmazer.Fulfilment.Jupiter.Password"]; ;
            this.zipStoragePath = ConfigurationManager.AppSettings["LetterAmazer.Settings.StoreZipPath"];
            this.pdfStoragePath = ConfigurationManager.AppSettings["LetterAmazer.Settings.StorePdfPath"];
            this.letterService = letterService;
            this.orderService = orderService;

            if (this.zipStoragePath.StartsWith("~")) // relative path
            {
                this.zipStoragePath = HttpContext.Current.Server.MapPath(this.zipStoragePath);
            }
            if (this.pdfStoragePath.StartsWith("~")) // relative path
            {
                this.pdfStoragePath = HttpContext.Current.Server.MapPath(this.pdfStoragePath);
            }
        }

        public void Process(IEnumerable<Letter> letters)
        {
            string zipPath = CreateZip(letters);
            logger.Debug("Zip path should be sent to Amazon: " + zipPath);
            string zipName = Path.GetFileName(zipPath);
            var s3 = GetS3Access();

            var amazonService = new AmazonS3Service(s3.KeyId, s3.SecretKey, "s3.amazonaws.com");
            using(FileStream fileStream = new FileStream(zipPath, FileMode.Open))
            {
                var md5valBase64 = HelperMethods.GetMD5HashFromStream(fileStream);
                fileStream.Position = 0;
                var md5val = HelperMethods.HashFile(fileStream);
                fileStream.Position = 0;
                amazonService.UploadFile(s3.Bucket, fileStream, zipName, md5valBase64);

                var sqsDoc = DeliveryXml(md5val, s3.Bucket, "Job run at " + DateTime.Now.ToString("yyMMdd-HHmmss"), zipName);

                amazonService.SendSQSMessage(sqsDoc.ToString(), s3.PostQueue);
            }

            orderService.UpdateByLetters(letters);

        }

        private XDocument DeliveryXml(string md5, string bucket, string description, string zipFileName)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), 
                new XElement("DELIVERY",
                new XElement("VERSION", 16),
                new XElement("DESCRIPTION", description),
                new XElement("ROUTINGMETRIC", "50052, 0, 0"),
                new XElement("JOBCODE", "RECONSPLITPDF"),
                new XElement("ZIPFILEPARTS", new XAttribute("MD5", md5), new XAttribute("bucket", bucket),
                new XElement("ZIPPART", zipFileName))));
            return doc;
        }

        private S3Access GetS3Access()
        {
            string accessCode = string.Empty;
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(this.username, this.password);
                accessCode = client.DownloadString(this.serviceUrl);
            }

            //XDocument doc = XDocument.Load(new MemoryStream(HelperMethods.GetBytes(accessCode)));


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(accessCode);

            var s3 = new S3Access()
            {
                User = doc["s3access"]["user"].InnerText,
                Bucket = doc["s3access"]["bucket"].InnerText,
                KeyId = doc["s3access"]["keyid"].InnerText,
                PostQueue = doc["s3access"]["postqueue"].InnerText,
                SecretKey = doc["s3access"]["secretkey"].InnerText,

            };
            if (string.IsNullOrEmpty(s3.PostQueue))
            {
                s3.PostQueue = "https://queue.amazonaws.com/375228553146/OrbitDelivery";
            }

            return s3;
        }

        private string CreateZip(IEnumerable<Letter> letters)
        {
            var zipName = this.username + "-" + DateTime.Now.ToString("yyMMdd-HHmmss") + ".zip";
            var zipPath = Path.Combine(this.zipStoragePath, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(zipPath))
            {
                Directory.CreateDirectory(zipPath);
            }
            var zipFilePath = Path.Combine(zipPath, zipName);

            var reconPath = Path.Combine(zipPath, "dtp-rec.csv");
            using (ZipFile zip = new ZipFile(zipFilePath))
            {
                FileStream fs = new FileStream(reconPath, FileMode.OpenOrCreate);
                StreamWriter wr = new StreamWriter(fs);
                wr.WriteLine("DocName,Country,Postcode,Copies,CompanyName,Document_Street1,Document_Street2,Document_Street3,City,State,Province,CountryName,AttPerson");
               
                foreach (var item in letters)
                {
                    zip.AddFile(Path.Combine(this.pdfStoragePath, item.LetterContent.Path), "/");

                    var fileName = Path.GetFileName(item.LetterContent.Path);
                    fileName = fileName.Trim('-');
                    logger.Debug("added file: " + fileName);
                    wr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                        fileName,
                        "\"" + item.ToAddress.Country.CountryCode + "\"", // TODO: Fix country
                        "\"" + item.ToAddress.Zipcode + "\"",
                        1,
                        string.Empty,
                        "\"" + item.ToAddress.Address1 + "\"",
                        string.Empty,
                        string.Empty,
                        "\"" + item.ToAddress.City + "\"",
                        string.Empty,
                        string.Empty,
                        "\"" + item.ToAddress.Country + "\"",
                        "\"" + item.ToAddress.FirstName + "\""));
                }
                
                wr.Close();
                fs.Close();
                zip.AddFile(reconPath, "/");
                zip.Save();
            }
            return zipFilePath;
        }
    }
}
