using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Utils.Helpers;
using log4net;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class PaypalMethod : IPaymentMethod
    {
        private IOrderService orderService;
        private static readonly ILog logger = LogManager.GetLogger(typeof(PaypalMethod));

        private string serviceUrl;
        private string paypalIpn;
        private string successUrl;
        private string baseUrl;


        public PaypalMethod(IOrderService orderService)
        {
            this.baseUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.BasePath");
            this.serviceUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.PayPal.ServiceUrl");
            this.paypalIpn = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.PayPal.IpnHandler");
            this.successUrl = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Successful");


            this.orderService = orderService;
        }
        public string Process(Order order)
        {
            var orderlinePayment = order.OrderLines.FirstOrDefault(c => c.ProductType == ProductType.Payment && 
                c.PaymentMethodId == 1);
            var orderlineProduct = order.OrderLines.FirstOrDefault(c => c.ProductType != ProductType.Payment);

            AddressInfo addressInfo = new AddressInfo();
            if (order.Customer != null && order.Customer.CustomerInfo != null)
            {
                addressInfo = order.Customer.CustomerInfo;
            }

            var totalPrice = orderlinePayment.Price.Total*orderlinePayment.Quantity;

            decimal volume = totalPrice;
            string firstName = addressInfo.FirstName;
            string lastName = addressInfo.LastName;
            string country = string.Empty;

            if (addressInfo.Country != null)
            {
                addressInfo.Country.CountryCode.ToString(); // TODO: Fix country    
            }

            string postal = addressInfo.Zipcode;
            string city = addressInfo.City;
            string address = addressInfo.Address1;
            var id = order.Id;
            string itemName = orderlineProduct.ProductType == ProductType.Letter ? "Send a letter" : "Letteramazer credits";
            string paypalIPNUrl = string.Format(this.paypalIpn, order.Id.ToString());
            var volumeForUsd = Math.Round(volume, 2).ToString().Replace(",", ".");
            var url = string.Format("{0}first_name={1}&item_name={2}&currency_code={3}&amount={4}&notify_url={5}&cmd=_xclick&country={6}&zip={7}&address1={8}&business={9}&city={10}&custom={11}&return={12}",
                this.serviceUrl,
                firstName,
                itemName,
                "USD",
                volumeForUsd,
                paypalIPNUrl,
                country,
                postal,
                address,
                Constants.Texts.PracticalInformation.PaypalEmail,
                city,
                id,
                successUrl);

            return url;
        }

        public void VerifyPayment(Order order)
        {
            order.OrderStatus = OrderStatus.Paid;
            order.DatePaid = DateTime.Now;

            orderService.ReplenishOrderLines(order);
            orderService.Update(order);
        }

        public void CallbackNotification()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.serviceUrl);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";


            logger.Info("IPN called");
            byte[] param = null; // try three time
            bool readSuccess = false;
            for (int i = 0; i < 3; i++)
            {
                if (readSuccess == true) break;
                try
                {
                    param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                    readSuccess = true;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    logger.DebugFormat("try {0}", i);
                }
            }

            logger.Info("Params length: " + param.Length);


            string strRequest = Encoding.ASCII.GetString(param);
            strRequest += "&cmd=_notify-validate";
            string strResponseCopy = strRequest;
            req.ContentLength = strRequest.Length;

            logger.Info("Strrequest: " + strRequest);


            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            logger.Info("StrResponse: " + strResponse);

            if (strResponse == "VERIFIED")
            {
                NameValueCollection theseArgies = HttpUtility.ParseQueryString(strResponseCopy);
                var id = theseArgies["custom"].ToString();
                logger.InfoFormat("IPN Verified, ORDER ID: {0}", id);

                int orderId = 0;
                int.TryParse(id, out orderId);

                if (orderId == 0)
                {
                    throw new BusinessException(string.Format("Cannot verify payment, Invalid OrderId: {0}", id));
                }

                logger.Info("Orderid int: " + orderId);

                var order = orderService.GetOrderById(orderId);
                if (order != null)
                {
                    VerifyPayment(order);
                    logger.Info("Order has been VerifyPayment");
                }
                else
                {
                    logger.Info("Order was null");
                }


            }
            else if (strResponse == "INVALID")
            {
                logger.InfoFormat("IPN Invlalid, Parameters: {0}", strRequest);
            }
        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
