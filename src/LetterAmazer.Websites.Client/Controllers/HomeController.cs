using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.Attributes;
using LetterAmazer.Websites.Client.ViewModels;
using System;
using System.Web.Mvc;
using LetterAmazer.Websites.Client.Extensions;
using log4net;
using System.Web.Security;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class HomeController : BaseController
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(HomeController));

        private ICountryService countryService;
        private ICustomerService customerService;
        private IOfficeService officeService;
        private IMailService mailService;
        private IPriceService priceService;

        public HomeController(ICustomerService customerService,IOfficeService officeService,
            IMailService mailService, ICountryService countryService, IPriceService priceService)
        {
            this.customerService = customerService;
            this.officeService = officeService;
            this.countryService = countryService;
            this.mailService = mailService;
            this.priceService = priceService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Faq()
        {
            mailService.ConfirmUser(new Customer()
            {
                Email = "mcoroklo@gmail.com",
                
            });
            return View();
        }

        public ActionResult Business()
        {
            return View();
        }

        public ActionResult ApiInfo()
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
            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            PriceViewModel prices = new PriceViewModel();
            foreach (var country in countries)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.Id.ToString(),

                };
                prices.Countries.Add(selectedItem);
            }

            return View(prices);
        }

        [HttpGet]
        public decimal GetPrice(int countryId, int pages)
        {
            var pricing = priceService.GetPriceBySpecification(new PriceSpecification()
            {
                CountryId = countryId,
                PageCount = pages
            });

            return pricing.Total;
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
                var customer = customerService.LoginUser(model.Email, model.Password);

                SessionHelper.Customer = customer;
                FormsAuthentication.SetAuthCookie(customer.Id.ToString(), model.Remember ?? false);

                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                
                return RedirectToAction("Index", "User");
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
            SessionHelper.Customer = null;
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

                var cust = customerService.Create(customer);

                SessionHelper.Customer = cust;
                FormsAuthentication.SetAuthCookie(cust.Id.ToString(), false);

                return RedirectToAction("CreateOrganisation", "User");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ModelState.AddBusinessError(ex.Message);
            }

            return RedirectToActionWithError("Register", model);
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
                Customer customer = customerService.GetCustomerBySpecification(new CustomerSpecification()
                {
                    ResetPasswordKey = key
                }).FirstOrDefault();

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

                Customer customer = customerService.GetCustomerBySpecification(new CustomerSpecification()
                {
                    ResetPasswordKey = model.ResetPasswordKey
                }).FirstOrDefault();

                customer.Password = model.Password;


                customerService.Update(customer);

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return RedirectToActionWithError("RecoverPassword", model);
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult Confirm(string key)
        {
            try
            {
                Customer customer = customerService.GetCustomerBySpecification(new CustomerSpecification()
                {
                    RegistrationKey = key
                }).FirstOrDefault();

                customerService.ActivateUser(customer);

                SessionHelper.Customer = customer;
                FormsAuthentication.SetAuthCookie(customer.Id.ToString(), true);

                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return new HttpStatusCodeResult(404);
        }
    }
}
