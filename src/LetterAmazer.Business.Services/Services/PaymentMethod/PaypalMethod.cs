using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LetterAmazer.Business.Services.Services.PaymentMethod
{
    public class PaypalMethod : IPaymentMethod
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PaypalMethod));
        public const string NAME = "Paypal";
        private string serviceUrl;
        private string paypalIPN;
        private string returnUrl;

        public PaypalMethod(string serviceUrl, string paypalIPN, string returnUrl)
        {
            this.serviceUrl = serviceUrl;
            this.paypalIPN = paypalIPN;
            this.returnUrl = returnUrl;
        }

        public string Name
        {
            get { return PaypalMethod.NAME; }
        }

        public string Process(OrderContext orderContext)
        {
            Order order = orderContext.Order;
            if (order == null || order.OrderItems == null || order.OrderItems.Count == 0) throw new BusinessException("Order can not be null!");

            AddressInfo addressInfo = order.OrderItems.ElementAt(0).Letter == null ? orderContext.Order.Customer.CustomerInfo : order.OrderItems.ElementAt(0).Letter.ToAddress;
            decimal volume = order.Price;
            string firstName = addressInfo.FirstName;
            string lastName = addressInfo.LastName;
            string country = addressInfo.Country.Value.ToString(); // TODO: Fix country
            string postal = addressInfo.Postal;
            string city = addressInfo.City;
            string address = addressInfo.Address;
            var id = order.Id;

            string paypalIPNUrl = string.Format(this.paypalIPN, orderContext.Order.OrderType.ToString());
            var volumeForUsd = Math.Round(volume, 2).ToString().Replace(",", ".");
            var url = string.Format("{0}first_name={1}&last_name={2}&item_name={3}&currency_code=USD&amount={4}&notify_url={5}&cmd=_xclick&country={6}&zip={7}&address1={8}&business={9}&city={10}&custom={11}&return={12}",
                this.serviceUrl, firstName, lastName, order.OrderType == OrderType.SendLetters ? "Send a single letter" : "Add Funds", 
                volumeForUsd, paypalIPNUrl, country, postal, address, "mcoroklo@gmail.com", city, 
                id, string.Format(this.returnUrl, orderContext.CurrentCulture));
            return url;
        }

        public VerifyPaymentResult Verify(VerifyPaymentContext context)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.serviceUrl);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            string strRequest = context.Parameters;
            strRequest += "&cmd=_notify-validate";
            string strResponseCopy = strRequest;
            req.ContentLength = strRequest.Length;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

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

                VerifyPaymentResult result = new VerifyPaymentResult();
                result.OrderId = orderId;
                result.Results = theseArgies;
                return result;
            }
            else if (strResponse == "INVALID")
            {
                logger.InfoFormat("IPN Invlalid, Parameters: {0}", context.Parameters);
            }

            throw new BusinessException(string.Format("Cannot verify payment, Parameters: {0}", context.Parameters));
        }
    }
}
