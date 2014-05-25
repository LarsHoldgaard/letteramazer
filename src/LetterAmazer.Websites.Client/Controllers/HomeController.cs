using System.Collections.Generic;
using System.IO;
using System.Linq;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Content;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.PriceUpdater;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.Attributes;
using LetterAmazer.Websites.Client.Helpers;
using LetterAmazer.Websites.Client.ViewModels;
using System;
using System.Web.Mvc;
using LetterAmazer.Websites.Client.Extensions;
using LetterAmazer.Websites.Client.ViewModels.Home;
using LetterAmazer.Websites.Client.ViewModels.Shared.Utils;
using LetterAmazer.Websites.Client.ViewModels.User;
using log4net;
using System.Web.Security;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class HomeController : BaseController
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(HomeController));

        private ICountryService countryService;
        private ICustomerService customerService;
        private IMailService mailService;
        private IPriceService priceService;
        private IOrganisationService organisationService;
        private IPriceUpdater priceUpdater;
        private IContentService contentService;
        public HomeController(ICustomerService customerService,IPriceUpdater priceUpdater,
            IMailService mailService, ICountryService countryService, IPriceService priceService, IOrganisationService organisationService,
            IContentService contentService)
        {
            this.customerService = customerService;
            this.countryService = countryService;
            this.mailService = mailService;
            this.priceService = priceService;
            this.organisationService = organisationService;
            this.priceUpdater = priceUpdater;
            this.contentService = contentService;
        }

        public ActionResult Index()
        {
            var windowedModel = new SendWindowedLetterViewModel()
            {
                PaymentMethodId = SessionHelper.Customer != null ? 2 : 1,
                LetterType = (int)LetterType.Windowed,
                UseUploadFile = true,
                IsLoggedIn = SessionHelper.Customer != null 
            };

            new Helper().FillCountries(windowedModel.Countries,59);

            return View(windowedModel);
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Business()
        {
            return View();
        }

        public ActionResult ApiInfo()
        {
            return View(new ApiViewModel());
        }

        [HttpPost]
        public ActionResult ApiInfo(ApiViewModel apiModel)
        {
            mailService.NotificationApiWish(apiModel.Email,apiModel.Organistion,apiModel.Comment);

            apiModel = new ApiViewModel();
            apiModel.Comment = string.Empty;
            apiModel.Organistion = string.Empty;
            apiModel.Email = string.Empty;
            apiModel.Status = "Thanks. We have received the e-mail and will get back to you soon";
            return View(apiModel);
        }

        public ActionResult Reseller()
        {
            ResellerViewModel model = new ResellerViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Reseller(ResellerViewModel model)
        {
            mailService.NotificationResellerWish(model.Email, model.Interest, model.Message);

            model = new ResellerViewModel();
            model.Interest = string.Empty;
            model.Message = string.Empty;
            model.Email = string.Empty;
            model.Status = "Thanks. We have received the e-mail and will get back to you soon";
            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            var prices = buildPriceViewModel(59); // ID of Denmark. TODO: some IP to countryID?

            
            PriceOverviewViewModel priceOverviewViewModel = new PriceOverviewViewModel();
            priceOverviewViewModel.PriceViewModel = prices;

            
            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            foreach (var country in countries)
            {
                priceOverviewViewModel.CountryPriceList.CountryPriceViewModel.Add(new CountryPriceViewModel()
                {
                    Alias = country.Alias,
                    Name = country.Name
                });
            }

            return View(priceOverviewViewModel);
        }


        public ActionResult GetSendALetterTo(string alias)
        {
            // TODO: clean up
            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            var windowedModel = new SendWindowedLetterViewModel()
            {
                PaymentMethodId = SessionHelper.Customer != null ? 2 : 1,
                LetterType = (int)LetterType.Windowed,
                UseUploadFile = true,
                IsLoggedIn = SessionHelper.Customer != null
            };

            var content = contentService.GetContentBySpecifications(new ContentSpecification()
            {
                Alias = alias,
                Section = "sendletter",
            }).FirstOrDefault();

            var country = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Alias = alias
            }).FirstOrDefault();

            var sendaletterto = new SendALetterToViewModel()
            {
                Content = content.Content,
                Headline = content.Headline,
                SeoTitle = string.IsNullOrEmpty(content.SeoTitle) ? content.Headline : content.SeoTitle,
                SendWindowedLetterViewModel = windowedModel
            };


            foreach (var acountry in countries)
            {
                sendaletterto.SendWindowedLetterViewModel.Countries.Add(new SelectListItem()
                {
                    Text = acountry.Name,
                    Value = acountry.Id.ToString()
                });

                sendaletterto.CountryPriceList.CountryPriceViewModel.Add(new CountryPriceViewModel()
                {
                    Alias = acountry.Alias,
                    Name = acountry.Name
                });
            }

            return View(sendaletterto);
        }

        public ActionResult GetPricing(string alias)
        {
            // TODO: clean up
            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });


            var content = contentService.GetContentBySpecifications(new ContentSpecification()
            {
                Alias = alias,
                Section = "price",
            }).FirstOrDefault();

            var country = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Alias = alias
            }).FirstOrDefault();

            var priceDescriptionViewModel = new PriceDescriptionViewModel()
            {
                Content = content.Content,
                Headline = content.Headline,
                PriceViewModel = buildPriceViewModel(country.Id),
                SeoTitle = string.IsNullOrEmpty(content.SeoTitle) ? content.Headline : content.SeoTitle
            };

            foreach (var acountry in countries)
            {
                priceDescriptionViewModel.CountryPriceList.CountryPriceViewModel.Add(new CountryPriceViewModel()
                {
                    Alias = acountry.Alias,
                    Name = acountry.Name
                });
            }

            return View(priceDescriptionViewModel);
        }

        [HttpGet]
        public decimal GetPrice(int countryId, int lettersize, int pages)
        {
            var letterSize = (LetterSize) lettersize;
            var pricing = priceService.GetPriceBySpecification(new PriceSpecification()
            {
                CountryId = countryId,
                PageCount = pages,
                LetterSize = letterSize
            });

            return pricing.PriceExVat;
        }


        public ActionResult Account()
        {
            AccountViewModel viewModel= new AccountViewModel();
            var helper = new Helper();
            helper.FillCountries(viewModel.RegisterViewModel.Countries,59);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel accountViewModel)
        {
            var model = accountViewModel.LoginViewModel;
            var customer = customerService.LoginUser(model.Email, model.Password);

            try
            {
                if (customer != null)
                {
                    SessionHelper.Customer = customer;
                    FormsAuthentication.SetAuthCookie(customer.Id.ToString(), model.Remember ?? false);

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "User");    
                }
                ModelState.AddBusinessError("Email and password combination is invalid");
                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ModelState.AddBusinessError("Email and password combination is invalid");
            }

            FormsAuthentication.SignOut();
            
            return RedirectToActionWithError("Account",accountViewModel);
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult Logout()
        {
            SessionHelper.Customer = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }


        [HttpPost]
        public ActionResult Register(AccountViewModel accountViewModel)
        {
            var model = accountViewModel.RegisterViewModel;

            try
            {
                Customer customer = new Customer();
                
                customer.Email = model.Email;
                customer.Password = model.Password;
                customer.CustomerInfo = new AddressInfo();
                customer.CustomerInfo.Country = countryService.GetCountryById(int.Parse(model.SelectedCountry));

                var cust = customerService.Create(customer);

                SessionHelper.Customer = cust;
                FormsAuthentication.SetAuthCookie(cust.Id.ToString(), false);

                if (cust.Organisation != null && cust.Organisation.Id > 0 && !cust.Organisation.IsPrivate)
                {
                    return RedirectToAction("EditOrganisation", "User");
                }
                if (cust.Organisation != null && cust.Organisation.Id > 0)
                {
                    cust.Organisation = null;
                    customerService.Update(cust);
                    this.organisationService.Delete(cust.Organisation);
                }

                return RedirectToAction("CreateOrganisation", "User");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ModelState.AddBusinessError(ex.Message);
            }

            return RedirectToActionWithError("Account", accountViewModel);
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
                if (ModelState.IsValid)
                {
                    customerService.RecoverPassword(model.Email);
                    return RedirectToAction("ForgotPasswordSuccess");
                }

            }
            catch (BusinessException businessException)
            {
                ModelState.AddBusinessError("We could not find any users with the provided e-mail address");
            }
            catch (Exception)
            {
                ModelState.AddBusinessError("We could not find any users with the provided e-mail address");
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

                return View(new RegisterViewModel() { ResetPasswordKey = key, Email = customer.Email });
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

                customer.Password = SHA1PasswordEncryptor.Encrypt(model.Password);


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

        #region Private helpers

        private PriceViewModel buildPriceViewModel(int standardCountryId)
        {
            
            PriceViewModel prices = new PriceViewModel();
           new Helper().FillCountries(prices.Countries,standardCountryId);
            
            prices.SelectedLetterSizes = ControllerHelpers.GetEnumSelectList<LetterSize>().ToList();
            prices.SelectedLetterSizes.RemoveAt(1);
            return prices;
        }


        #endregion
    }
}
