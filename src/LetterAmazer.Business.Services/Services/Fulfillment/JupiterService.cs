using Ionic.Zip;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using LetterAmazer.Business.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace LetterAmazer.Business.Services.Services.Fulfillment
{
    public class JupiterService : IFulfillmentService
    {
        private string zipStoragePath;
        private string pdfStoragePath;
        private string username;
        private string password;
        private string serviceUrl;
        public JupiterService(string username, string password, string serviceUrl, string zipStoragePath, string pdfStoragePath)
        {
            this.serviceUrl = serviceUrl;
            this.username = username;
            this.password = password;
            this.zipStoragePath = zipStoragePath;
            this.pdfStoragePath = pdfStoragePath;

            if (this.zipStoragePath.StartsWith("~")) // relative path
            {
                this.zipStoragePath = HttpContext.Current.Server.MapPath(this.zipStoragePath);
            }
            if (this.pdfStoragePath.StartsWith("~")) // relative path
            {
                this.pdfStoragePath = HttpContext.Current.Server.MapPath(this.pdfStoragePath);
            }
        }

        public void Process(IList<Order> orders)
        {
            string zipPath = CreateZip(orders);
            string zipName = Path.GetFileName(zipPath);
            var s3 = GetS3Access();

            var amazonService = new AmazonS3Service(s3.keyid, s3.secretkey, "s3.amazonaws.com");
            var fileStream = new FileStream(zipPath, FileMode.Open); //new MemoryStream(HelperMethods.GetBytes(zipPath));

            var md5valBase64 = HelperMethods.GetMD5HashFromStream(fileStream);
            var md5val = HelperMethods.HashFile(fileStream);
            fileStream.Position = 0;
            amazonService.UploadFile(s3.bucket, fileStream, zipName, md5valBase64);

            var sqsDoc = DeliveryXml(md5val, s3.bucket, "Job run at " + DateTime.Now.ToString("yyMMdd-HHmmss"), zipName);

            amazonService.SendSQSMessage(sqsDoc.ToString(), s3.postqueue);
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

        private s3access GetS3Access()
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

            var s3 = new s3access()
            {
                user = doc["s3access"]["user"].InnerText,
                bucket = doc["s3access"]["bucket"].InnerText,
                keyid = doc["s3access"]["keyid"].InnerText,
                postqueue = doc["s3access"]["postqueue"].InnerText,
                secretkey = doc["s3access"]["secretkey"].InnerText,

            };
            if (string.IsNullOrEmpty(s3.postqueue))
            {
                s3.postqueue = "https://queue.amazonaws.com/375228553146/OrbitDelivery";
            }

            return s3;
        }

        private string CreateZip(IList<Order> orders)
        {
            var zipName = this.username + "-" + DateTime.Now.ToString("yyMMdd-HHmmss") + ".zip";
            var zipPath = Path.Combine(this.zipStoragePath, DateTime.Now.ToString("yyyyMMdd"));
            var zipFilePath = Path.Combine(zipPath, zipName);

            var reconPath = Path.Combine(zipPath, "dtp-rec.csv");
            using (ZipFile zip = new ZipFile(zipFilePath))
            {
                FileStream fs = new FileStream(reconPath, FileMode.OpenOrCreate);
                StreamWriter wr = new StreamWriter(fs);
                wr.WriteLine("DocName,Country,Postcode,Copies,CompanyName,Document_Street1,Document_Street2,Document_Street3,City,State,Province,CountryName,AttPerson");
                foreach (var order in orders)
                {
                    foreach (var item in order.OrderItems)
                    {
                        zip.AddFile(Path.Combine(this.pdfStoragePath, item.Letter.LetterContent.Path));

                        wr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                            item.Letter.LetterContent.Path,
                            item.Letter.ToAddress.CountryCode,
                            item.Letter.ToAddress.Postal,
                            1,
                            string.Empty,
                            item.Letter.ToAddress.Address,
                            string.Empty,
                            string.Empty,
                            item.Letter.ToAddress.City,
                            string.Empty,
                            string.Empty,
                            item.Letter.ToAddress.Country,
                            item.Letter.ToAddress.FirstName));
                    }
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
