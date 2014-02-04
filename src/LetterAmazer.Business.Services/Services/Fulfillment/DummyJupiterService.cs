using Ionic.Zip;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Orders;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace LetterAmazer.Business.Services.Services.Fulfillment
{
    public class DummyJupiterService : IFulfillmentService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DummyJupiterService));
        private string zipStoragePath;
        private string pdfStoragePath;

        public DummyJupiterService(string zipStoragePath, string pdfStoragePath)
        {
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
        }

        private string CreateZip(IList<Order> orders)
        {
            var zipName = "DUMMY-" + DateTime.Now.ToString("yyMMdd-HHmmss") + ".zip";
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
                foreach (var order in orders)
                {
                    foreach (var item in order.Letters)
                    {
                        zip.AddFile(Path.Combine(this.pdfStoragePath, item.LetterContent.Path), "/");

                        wr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}",
                            Path.GetFileName(item.LetterContent.Path),
                            item.ToAddress.Country.Name, // TODO: Fix country
                            item.ToAddress.PostalCode,
                            1,
                            string.Empty,
                            item.ToAddress.Address1,
                            string.Empty,
                            string.Empty,
                            item.ToAddress.City,
                            string.Empty,
                            string.Empty,
                            item.ToAddress.Country,
                            item.ToAddress.FirstName));
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
