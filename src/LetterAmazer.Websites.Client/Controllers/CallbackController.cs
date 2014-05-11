using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Services.PaymentMethods.Implementations;
using LetterAmazer.Business.Utils.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class CallbackController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CallbackController));
        private string serviceUrl;
        private IPaymentService paymentService;
        private IPriceService priceService;
        private IOrderService orderService;
        private ICustomerService customerService;

        public CallbackController(IPaymentService paymentService, IPriceService priceService, IOrderService orderService, ICustomerService customerService)
        {
            this.paymentService = paymentService;
            this.priceService = priceService;
            this.serviceUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.PayPal.ServiceUrl");
            this.orderService = orderService;
            this.customerService = customerService;
        }

        //
        // GET: /Callback/


        public JsonResult PaypalIpn(string abekat)
        {
            try
            {
                // TODO: move to payment service layer

                logger.Info("IPN called");

                PaypalMethod paypalMethod = new PaypalMethod(orderService);

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
                        param = Request.BinaryRead(Request.ContentLength);
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
                        paypalMethod.VerifyPayment(order);
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

                var updated_customer = customerService.Update(SessionHelper.Customer);
                SessionHelper.Customer = updated_customer;
                FormsAuthentication.SetAuthCookie(SessionHelper.Customer.Id.ToString(), true);


                return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
