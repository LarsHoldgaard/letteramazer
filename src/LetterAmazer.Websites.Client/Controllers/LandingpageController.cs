using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.Extensions;
using LetterAmazer.Websites.Client.Helpers;
using LetterAmazer.Websites.Client.ViewModels;
using LetterAmazer.Websites.Client.ViewModels.Home;
using LetterAmazer.Websites.Client.ViewModels.LandingPage;
using LetterAmazer.Websites.Client.ViewModels.Shared.Utils;
using log4net;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class LandingpageController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LandingpageController));

        private ICountryService countryService;
        private ICustomerService customerService;
        private IOrganisationService organisationService;

        public LandingpageController(ICountryService countryService, ICustomerService customerService,
            IOrganisationService organisationService)
        {
            this.countryService = countryService;
            this.customerService = customerService;
            this.organisationService = organisationService;
        }


        #region E-conomic
        public ActionResult EconomicDk()
        {
            var vm = new EconomicDkViewModel();

            Helper.FillCountries(countryService, vm.Countries, 59);
            return View(vm);
        }
        
        private string getEconomicAuthUrl(string customerId)
        {
            var publicAppId = ConfigurationManager.AppSettings.Get("LetterAmazer.Apps.Economics.PublicAppId");
            var baseUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.BasePath");
            var returnUrl = baseUrl + ConfigurationManager.AppSettings["LetterAmazer.Apps.Economics.ReturnUrl"] + "?status=new," + customerId;
            var economicAuthUrl = string.Format(ConfigurationManager.AppSettings.Get("LetterAmazer.Apps.Economics.PermissionUrl"),
                returnUrl, publicAppId);
            return economicAuthUrl;
        }

        [HttpPost]
        public ActionResult EconomicDk(string submitBtn, EconomicDkViewModel vm)
        {
            var countryId = int.Parse(vm.SelectedCountry);
            

            Customer customer = new Customer();

            switch (submitBtn)
            {
                case "Opret gratis bruger":
                    try
                    {
                        customer.Email = vm.Email;
                        customer.Password = vm.Password;
                        customer.CustomerInfo = new AddressInfo();
                        customer.CustomerInfo.Country = countryService.GetCountryById(countryId);

                        var cust = customerService.Create(customer);

                        SessionHelper.Customer = cust;
                        FormsAuthentication.SetAuthCookie(cust.Id.ToString(), false);

                        if (cust.Organisation != null && cust.Organisation.Id > 0 && !cust.Organisation.IsPrivate)
                        {
                            return RedirectToAction("EditOrganisation", "User", new { returnUrl = getEconomicAuthUrl(cust.Id.ToString()) });
                        }
                        if (cust.Organisation != null && cust.Organisation.Id > 0)
                        {
                            cust.Organisation = null;
                            customerService.Update(cust);
                            this.organisationService.Delete(cust.Organisation);
                        }

                        return RedirectToAction("CreateOrganisation", "User", new { returnUrl = getEconomicAuthUrl(cust.Id.ToString()) });
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        ModelState.AddBusinessError(ex.Message);
                    }

                    break;
                case "Log ind":
                    var user = customerService.LoginUser(vm.Email, vm.Password);
                    SessionHelper.Customer = customer;
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(),true);
                    return Redirect(getEconomicAuthUrl(user.Id.ToString()));
            }


            return RedirectToActionWithError("EconomicDk", vm);
        }

        #endregion

        #region Adwords landing pages 

        public ActionResult LmPostDk()
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

        private PriceViewModel buildPriceViewModel(int standardCountryId)
        {

            PriceViewModel prices = new PriceViewModel();
            Helper.FillCountries(countryService, prices.Countries, standardCountryId);

            prices.SelectedLetterSizes = ControllerHelpers.GetEnumSelectList<LetterSize>().ToList();
            prices.SelectedLetterSizes.RemoveAt(1);
            return prices;
        }


        #endregion
    }
}
