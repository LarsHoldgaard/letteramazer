using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Domain.Partners.PartnerJsonDto;
using Newtonsoft.Json;

namespace LetterAmazer.Business.Services.Services.Partners.Invoice
{
    public class EconomicInvoiceService:IPartnerInvoiceService
    {
        private string accessId;
        private string apiUrl;
        private string privateId;

        public EconomicInvoiceService()
        {
            this.accessId = ConfigurationManager.AppSettings["LetterAmazer.Apps.Economics.AccessId"];
            this.apiUrl = ConfigurationManager.AppSettings["LetterAmazer.Apps.Economics.ApiUrl"];
            this.privateId = ConfigurationManager.AppSettings["LetterAmazer.Apps.Economics.PrivateAppId"];
        }

        public List<PartnerInvoice> GetBySpecification(PartnerInvoiceSpecification partnerInvoiceSpecification)
        {
            var invoiceApiUrl = string.Format("{0}/{1}", apiUrl, "invoices/booked?pageSize=999");

            var invoiceString = getJsonStringFromRequest(buildEconomicsHttpRequest(invoiceApiUrl));
            var economicsInvoices = JsonConvert.DeserializeObject<EconomicsPartnerInvoices>(invoiceString);
            var collection =
                economicsInvoices.collection.Where(
                    c => c.date >= partnerInvoiceSpecification.From && c.date <= partnerInvoiceSpecification.To).OrderByDescending(c=>c.date);

            var invoices = new List<PartnerInvoice>();
            foreach (var economicInvoice in collection)
            {
                invoices.Add(new PartnerInvoice()
                {
                    DateCreated = economicInvoice.date,
                    PdfUrl = economicInvoice.pdf,
                    Id = economicInvoice.orderId
                });
            }

            return invoices;
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

        private HttpWebRequest buildEconomicsHttpRequest(string url)
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
