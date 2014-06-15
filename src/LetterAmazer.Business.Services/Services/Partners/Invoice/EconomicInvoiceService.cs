using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LetterAmazer.Business.Services.Domain.Files;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Domain.Partners.PartnerJsonDto;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Utils;
using Newtonsoft.Json;

namespace LetterAmazer.Business.Services.Services.Partners.Invoice
{
    public class EconomicInvoiceService:IPartnerInvoiceService
    {
        private string apiUrl;
        private string privateId;
        private IPartnerService partnerService;
        private IFileService fileService;
        

        public EconomicInvoiceService(IPartnerService partnerService, IFileService fileService)
        {
            this.apiUrl = ConfigurationManager.AppSettings["LetterAmazer.Apps.Economics.ApiUrl"];
            this.privateId = ConfigurationManager.AppSettings["LetterAmazer.Apps.Economics.PrivateAppId"];
            this.partnerService = partnerService;
            this.fileService = fileService;
        }

        public PartnerInvoice GetPartnerInvoiceById(string accessId, string id)
        {
            var invoiceApiUrl = string.Format("{0}/{1}/{2}", apiUrl, "invoices/booked",id);

            var invoiceString = getJsonStringFromRequest(buildEconomicsHttpRequest(invoiceApiUrl,accessId));
            var economicsInvoice = JsonConvert.DeserializeObject<EconomicInvoice>(invoiceString);

            return getPartnerInvoice(economicsInvoice);
        }

        public List<PartnerInvoice> GetPartnerInvoiceBySpecification(string accessId, PartnerInvoiceSpecification partnerInvoiceSpecification)
        {
            var partnerAccess = partnerService.GetPartnerAccessBySpecification(new PartnerAccessSpecification()
            {
                PartnerId = partnerInvoiceSpecification.PartnerId,
                Token = accessId
            }).FirstOrDefault();

            if (partnerAccess == null)
            {
                throw new ArgumentException("No partnerAccess could be found");
            }

            var invoiceApiUrl = string.Format("{0}/{1}", apiUrl, "invoices/booked?pageSize=999");

            var invoiceString = getJsonStringFromRequest(buildEconomicsHttpRequest(invoiceApiUrl,accessId));
            var economicsInvoices = JsonConvert.DeserializeObject<EconomicsPartnerInvoices>(invoiceString); 
           
            var collection =
                economicsInvoices.collection.Where(
                    c => c.date >= partnerInvoiceSpecification.From && c.date <= partnerInvoiceSpecification.To).OrderByDescending(c=>c.date);


            var invoices = new List<PartnerInvoice>();
            foreach (var economicInvoice in collection)
            {
                var partnerTransactions = partnerService.GetPartnerTransactionBySpecification(new PartnerTransactionSpecification()
                {
                    CustomerId = partnerAccess.UserId,
                    PartnerId = partnerInvoiceSpecification.PartnerId,
                    ValueId = int.Parse(economicInvoice.id)
                }).FirstOrDefault();

                var partnerInvoice = getPartnerInvoice(economicInvoice, partnerTransactions);
                invoices.Add(partnerInvoice);
            }

            return invoices;
        }

        public byte[] GetEconomicPdfBytes(string accessId,string pdfUrl)
        {
            var request = buildEconomicsHttpRequest(pdfUrl, accessId);
            var response = request.GetResponse();
            var data = new byte[0];

            using (Stream stream = response.GetResponseStream())
            {
                data= Helpers.GetBytes(stream);
            }

            return data;
        }

        private PartnerInvoice getPartnerInvoice(EconomicInvoice economicInvoice, PartnerTransaction partnerTransaction = null)
        {
            return new PartnerInvoice()
            {
                CustomerName = economicInvoice.customerName,
                PdfUrl = economicInvoice.self + "/pdf", // this is a PDF link that requires access tokens
                InvoiceDate = economicInvoice.date,
                LetterAmazerStatus = partnerTransaction != null,
                Id = economicInvoice.id,
                OrderId = economicInvoice.orderId,
                Price = new Price()
                {
                    PriceExVat = economicInvoice.netAmount
                },
                CustomerAddress = economicInvoice.customerAddress,
                CustomerCity = economicInvoice.customerCity,
                CustomerCountry = economicInvoice.customerCountry,
                CustomerCounty = economicInvoice.customerCounty,
                CustomerPostalCode = economicInvoice.customerPostalCode
            };
        }

        private string getJsonStringFromRequest(HttpWebRequest request)
        {
            try
            {
                var response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();
                    return responseString;
                }
            }
            catch (Exception)
            {
                // TODO: give back some error - we know economics can be down from time to time :p
                throw new Exception();
            }
            
        }

        private HttpWebRequest buildEconomicsHttpRequest(string url, string accessId)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers.Add("accessId", accessId);
            req.Headers.Add("appId", privateId);
            req.Method = "GET";
            req.ContentType = "application/json";
            req.Accept = "application/json";
            return req;
        }
    }
}
