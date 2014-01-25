using LetterAmazer.Websites.Client.Attributes;
using LetterAmazer.Websites.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using LetterAmazer.Websites.Client.Extensions;
using log4net;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Interfaces;
using System.Web.Security;
using LetterAmazer.Websites.Client.Resources.Views.Home;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class HomeController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(HomeController));
        private ICustomerService customerService;
        public HomeController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Business()
        {
            return View();
        }

        public ActionResult Api()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                Customer user = customerService.ValidateUser(model.Email, model.Password);

                FormsAuthentication.SetAuthCookie(user.Id.ToString(), model.Remember ?? false);

                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ModelState.AddBusinessError(LetterAmazer.Websites.Client.Resources.Views.Shared.ViewRes.EmailOrPasswordInvalid);
            }

            FormsAuthentication.SignOut();
            return RedirectToActionWithError("Login", model);
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                Customer customer = new Customer();
                customer.Email = model.Email;
                customer.Password = model.Password;
                customer.CustomerInfo = new AddressInfo();
                customer.CustomerInfo.FirstName = model.FirstName;
                customer.CustomerInfo.LastName = model.LastName;
                customer.CustomerInfo.CompanyName = model.Organization;
                customerService.CreateCustomer(customer);

                return RedirectToAction("RegisterSuccess");
            }
            catch (Exception ex)
            {
                ModelState.AddBusinessError(ex.Message);
            }

            return RedirectToActionWithError("Register", model);
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult ForgotPassword()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ForgotPassword(LoginViewModel model)
        {
            try
            {
                customerService.RecoverPassword(model.Email);

                return RedirectToAction("ForgotPasswordSuccess");
            }
            catch (Exception)
            {
                ModelState.AddBusinessError(LetterAmazer.Websites.Client.Resources.Views.Shared.ViewRes.EmailInvalid);
            }

            return RedirectToActionWithError("ForgotPassword", model);
        }

        public ActionResult ForgotPasswordSuccess()
        {
            return View();
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult RecoverPassword(string key)
        {
            try
            {
                Customer customer = customerService.GetUserByResetPasswordKey(key);

                return View(new RegisterViewModel() { ResetPasswordKey = key });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        public ActionResult RecoverPassword(RegisterViewModel model)
        {
            try
            {
                customerService.ResetPassword(model.ResetPasswordKey, model.Password);

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return RedirectToActionWithError("RecoverPassword", model);
        }
    }
}
