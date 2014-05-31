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
                logger.Info("IPN called");

                PaypalMethod paypalMethod = new PaypalMethod(orderService);

                paypalMethod.CallbackNotification();

                //var updated_customer = customerService.Update(SessionHelper.Customer);
                //SessionHelper.Customer = updated_customer;
                //FormsAuthentication.SetAuthCookie(SessionHelper.Customer.Id.ToString(), true);


                return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult BitPay(string content)
        {
            var method = new EpayMethod();

            try
            {
                method.CallbackNotification();

                return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Epay(string content)
        {
            var method = new EpayMethod();

            try
            {
                method.CallbackNotification();

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
