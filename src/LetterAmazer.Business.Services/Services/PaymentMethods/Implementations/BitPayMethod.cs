using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Utils;
using Newtonsoft.Json;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class BitPayMethod : IPaymentMethod
    {
        private string apiKey;
        private string apiUrl;
        private string callbackUrl;
        private string notificaitonEmail;
        private string successfulUrl;

        public BitPayMethod()
        {
            apiKey = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.Apikey");
            apiUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.ApiUrl");
            callbackUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.CallbackUrl");
            notificaitonEmail = ConfigurationManager.AppSettings.Get("LetterAmazer.Notification.Emails");
            successfulUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Successful");
        }

        public string Process(Order order)
        {
            var postUrl = string.Format("{0}/{1}", apiUrl, "invoice");

            var model = new BitPayInvoiceViewModel()
            {
                currency = "EUR",
                price = order.Price.Total,
                //notificationURL = callbackUrl,
                orderID = order.Id.ToString(),
                //notificationEmail = notificaitonEmail,
                //redirectURL = successfulUrl
            };

            var jsonVal = JsonConvert.SerializeObject(model);
            var jsonBytes = Helpers.GetBytes(jsonVal);

            var strResponse = string.Empty;


            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.UseDefaultCredentials = true;
            request.ContentType = "application/json";
            request.Credentials = new NetworkCredential(apiKey,string.Empty);
            request.Method = "POST";
            request.ContentLength = jsonBytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(jsonBytes,0,jsonBytes.Length);
            }

            var responseRequest = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(responseRequest.GetResponseStream()).ReadToEnd();

            ////var data = client.UploadData(postUrl, jsonBytes);
                //strResponse = Helpers.GetString(data);

                
            
            var response = JsonConvert.DeserializeObject<BitPayInvoiceCreatedViewModel>(strResponse);

            return response.url;
        }

        public void VerifyPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public void CallbackNotification()
        {
            throw new NotImplementedException();
        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }

    }

    public class BitPayInvoiceCreatedViewModel
    {
        public string id { get; set; }
        public string url { get; set; }
        public string status { get; set; }
       
    }

    public class BitPayInvoiceViewModel
    {
        public decimal price { get; set; }
        public string currency { get; set; }
        //public string notificationURL { get; set; }
        //public string notificationEmail { get; set; }
        public string orderID { get; set; }
        //public string redirectURL { get; set; }
    }

}
