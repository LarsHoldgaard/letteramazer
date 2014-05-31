using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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


        private IOrderService orderService;

        public BitPayMethod(IOrderService orderService)
        {
            apiKey = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.Apikey");
            apiUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.ApiUrl");
            callbackUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.CallbackUrl");
            notificaitonEmail = ConfigurationManager.AppSettings.Get("LetterAmazer.Notification.Emails");
            successfulUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Successful");
            this.orderService = orderService;
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
            request.Referer = "http://www.letteramazer.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36";
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
            int id = 0;
            int.TryParse(HttpContext.Current.Request.QueryString["orderid"], out id);

            var order = orderService.GetOrderById(id);
            order.OrderStatus = OrderStatus.Paid;
            order.DatePaid = DateTime.Now;
            orderService.ReplenishOrderLines(order);
            orderService.Update(order);
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
