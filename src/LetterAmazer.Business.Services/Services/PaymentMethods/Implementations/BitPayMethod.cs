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
        private string baseUrl;
        private string apiKey;
        private string apiUrl;
        private string callbackUrl;
        private string notificaitonEmail;
        private string successfulUrl;


        private IOrderService orderService;

        public BitPayMethod(IOrderService orderService)
        {
            this.baseUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.BasePath");
            apiKey = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.Apikey");
            apiUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.ApiUrl");
            callbackUrl = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Bitpay.CallbackUrl");
            notificaitonEmail = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Notification.Emails");
            successfulUrl = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Successful");
            this.orderService = orderService;
        }

        public string Process(Order order)
        {
            var postUrl = string.Format("{0}/{1}", apiUrl, "invoice");

            var model = new BitPayInvoiceViewModel()
            {
                currency = "EUR",
                price = order.Price.Total,
                notificationURL = callbackUrl,
                orderID = order.Id.ToString(),
                notificationEmail = notificaitonEmail,
                redirectURL = successfulUrl
            };

            var jsonVal = JsonConvert.SerializeObject(model);

            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.ContentType = "application/json";
            request.Credentials = new NetworkCredential(apiKey, string.Empty);
            request.Method = "POST";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(jsonVal);
                stream.Flush();
                stream.Close();
            }

            var responseRequest = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(responseRequest.GetResponseStream()).ReadToEnd();

            var response = JsonConvert.DeserializeObject<BitPayInvoiceCreatedViewModel>(responseString);

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
        public string btcPrice { get; set; }
        public float price { get; set; }
        public string currency { get; set; }
        public long invoiceTime { get; set; }
        public long expirationTime { get; set; }
        public long currentTime { get; set; }
        public string btcPaid { get; set; }
        public float rate { get; set; }
        public bool exceptionStatus { get; set; }

    }

    public class BitPayInvoiceViewModel
    {
        public decimal price { get; set; }
        public string currency { get; set; }
        public string notificationURL { get; set; }
        public string notificationEmail { get; set; }
        public string orderID { get; set; }
        public string redirectURL { get; set; }
    }

}
