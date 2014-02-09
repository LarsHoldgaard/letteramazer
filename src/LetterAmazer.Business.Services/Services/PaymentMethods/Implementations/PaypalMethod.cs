using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Exceptions;
using log4net;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class PaypalMethod : IPaymentMethod
    {
        //private static readonly ILog logger = LogManager.GetLogger(typeof(PaypalMethod));
        //public const string NAME = "Paypal";
        //private string serviceUrl;
        //private string paypalIPN;
        //private string returnUrl;

        //public PaypalMethod(string serviceUrl, string paypalIPN, string returnUrl)
        //{
        //    this.serviceUrl = serviceUrl;
        //    this.paypalIPN = paypalIPN;
        //    this.returnUrl = returnUrl;
        //}

        //public string Name
        //{
        //    get { return PaypalMethod.NAME; }
        //}

        //public string Process(OrderContext orderContext)
        //{
        //    Order order = orderContext.Order;
        //    if (order == null || order.Letters == null || order.Letters.Count == 0) throw new BusinessException("Order can not be null!");

        //    var addressInfo = order.Letters.ElementAt(0) == null ? orderContext.Order.Customer.CustomerInfo : order.Letters.ElementAt(0).ToAddress;
        //    decimal volume = order.Price;
        //    string firstName = addressInfo.FirstName;
        //    string lastName = addressInfo.LastName;
        //    string country = addressInfo.Country.CountryCode.ToString(); // TODO: Fix country
        //    string postal = addressInfo.PostalCode;
        //    string city = addressInfo.City;
        //    string address = addressInfo.Address1;
        //    var id = order.Id;

        //    string paypalIPNUrl = string.Format(this.paypalIPN, orderContext.Order.OrderType.ToString());
        //    var volumeForUsd = Math.Round(volume, 2).ToString().Replace(",", ".");
        //    var url = string.Format("{0}first_name={1}&last_name={2}&item_name={3}&currency_code=USD&amount={4}&notify_url={5}&cmd=_xclick&country={6}&zip={7}&address1={8}&business={9}&city={10}&custom={11}&return={12}",
        //        this.serviceUrl, firstName, lastName, order.OrderType == (int)OrderType.SendLetter ? "Send a single letter" : "Add Funds", 
        //        volumeForUsd, paypalIPNUrl, country, postal, address, "mcoroklo@gmail.com", city, 
        //        id, string.Format(this.returnUrl, orderContext.CurrentCulture));
        //    return url;
        //}

        //public VerifyPaymentResult Verify(VerifyPaymentContext context)
        //{
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.serviceUrl);

        //    //Set values for the request back
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";

        //    string strRequest = context.Parameters;
        //    strRequest += "&cmd=_notify-validate";
        //    string strResponseCopy = strRequest;
        //    req.ContentLength = strRequest.Length;

        //    //Send the request to PayPal and get the response
        //    StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
        //    streamOut.Write(strRequest);
        //    streamOut.Close();
        //    StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
        //    string strResponse = streamIn.ReadToEnd();
        //    streamIn.Close();

        //    if (strResponse == "VERIFIED")
        //    {
        //        NameValueCollection theseArgies = HttpUtility.ParseQueryString(strResponseCopy);
        //        var id = theseArgies["custom"].ToString();
        //        logger.InfoFormat("IPN Verified, ORDER ID: {0}", id);

        //        int orderId = 0;
        //        int.TryParse(id, out orderId);

        //        if (orderId == 0)
        //        {
        //            throw new BusinessException(string.Format("Cannot verify payment, Invalid OrderId: {0}", id));
        //        }

        //        VerifyPaymentResult result = new VerifyPaymentResult();
        //        result.OrderId = orderId;
        //        result.Results = theseArgies;
        //        return result;
        //    }
        //    else if (strResponse == "INVALID")
        //    {
        //        logger.InfoFormat("IPN Invlalid, Parameters: {0}", context.Parameters);
        //    }

        //    throw new BusinessException(string.Format("Cannot verify payment, Parameters: {0}", context.Parameters));
        //}

        public void Process(IPurchasable purchasable)
        {
            var customer = purchasable.GetCustomerDetails();
            var addressInfo = customer.CustomerInfo;
            var totalPrice = purchasable.TotalPrice();
            var productTitle = purchasable.PurchaseText();

            decimal volume = totalPrice;
            string firstName = addressInfo.FirstName;
            string lastName = addressInfo.LastName;
            string country = addressInfo.Country.CountryCode.ToString(); // TODO: Fix country
            string postal = addressInfo.PostalCode;
            string city = addressInfo.City;
            string address = addressInfo.Address1;
            var id = order.Id;

            string paypalIPNUrl = string.Format(this.paypalIPN, orderContext.Order.OrderType.ToString());
            var volumeForUsd = Math.Round(volume, 2).ToString().Replace(",", ".");
            var url = string.Format("{0}first_name={1}&last_name={2}&item_name={3}&currency_code=USD&amount={4}&notify_url={5}&cmd=_xclick&country={6}&zip={7}&address1={8}&business={9}&city={10}&custom={11}&return={12}",
                this.serviceUrl, firstName, lastName, productTitle,
                volumeForUsd, paypalIPNUrl, country, postal, address, "mcoroklo@gmail.com", city,
                id, string.Format(this.returnUrl, orderContext.CurrentCulture));
            
            return url;
        }

        public void VerifyPayment(IPurchasable purchasable)
        {
            throw new NotImplementedException();
        }

        public void ChargeBacks(IPurchasable purchasable)
        {
            throw new NotImplementedException();
        }
    }
}
