using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using Castle.Windsor.Diagnostics.Extensions;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Checkout;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Files;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Services.Partners;
using LetterAmazer.Business.Services.Services.Partners.Invoice;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.Helpers;
using LetterAmazer.Websites.Client.ViewModels;
using LetterAmazer.Websites.Client.ViewModels.Partner;
using log4net;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class PartnerController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PartnerController));

        private IOrderService orderService;
        private IPaymentService paymentService;
        private EconomicInvoiceService economicInvoiceService;
        private ICheckoutService checkoutService;
        private IOfficeProductService officeProductService;
        private IPriceService priceService;
        private ICountryService countryService;
        private IFileService fileService;
        private IPartnerService partnerService;
        private ICustomerService customerService;

        public PartnerController(IOrderService orderService, IPaymentService paymentService, ICheckoutService checkoutService,
            IOfficeProductService officeProductService, IPriceService priceService, ICountryService countryService, IFileService fileService, EconomicInvoiceService economicInvoiceService,
            IPartnerService partnerService, ICustomerService customerService)
        {
            this.economicInvoiceService = economicInvoiceService;
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.checkoutService = checkoutService;
            this.officeProductService = officeProductService;
            this.priceService = priceService;
            this.countryService = countryService;
            this.fileService = fileService;
            this.partnerService = partnerService;
            this.customerService = customerService;
        }

        public ActionResult Economic(string token, string status = "", string fromDateInput = "", string toDateInput = "")
        {
            // TODO: would someone please clean up this 100000 line long code? at least make subroutines?

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("EconomicDk", "Landingpage");
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (string.IsNullOrEmpty(status) || !status.Contains(","))
                {
                    return RedirectToAction("EconomicDk", "Landingpage");
                }


                var split = status.Split(',');
                status = split[0];
                var userid = split[1];
                if (status == "new")
                {
                    var ps = partnerService.GetPartnerAccessBySpecification(new PartnerAccessSpecification()
                    {
                        PartnerId = 1,
                        Token = token
                    });

                    if (ps.Count == 0)
                    {
                        // create partnerAccess
                        partnerService.Create(new PartnerAccess()
                        {
                            AccessId = token,
                            PartnerId = 1,
                            UserId = int.Parse(userid)
                        });
                    }


                }
            }


            var baseUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.BasePath");
            var p = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Apps.Economics.ReturnUrl") + "?token=" + token;

            var access = partnerService.GetPartnerAccessBySpecification(new PartnerAccessSpecification()
            {
                PartnerId = 1,
                Token = token
            }).FirstOrDefault();
            var user = customerService.GetCustomerById(access.UserId);
            SessionHelper.Customer = user;
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);



            var model = new PartnerInvoiceOverviewViewModel()
            {
                AccountStatus = status,
                Token = token,
                AppUrl = p,
                UserId = access.UserId,
                UserCredits = user.CreditsLeft,
                AccessToken = access.AccessId
            };

            if (!string.IsNullOrEmpty(fromDateInput))
            {
                model.From = DateTime.Parse(fromDateInput);
            }
            if (!string.IsNullOrEmpty(toDateInput))
            {
                model.To = DateTime.Parse(toDateInput);
            }


            var invoices = economicInvoiceService.GetPartnerInvoiceBySpecification(token, new PartnerInvoiceSpecification()
            {
                From = model.From,
                To = model.To,
            });

            Helper.FillCountries(countryService, model.Countries, 59);
            Helper.FillPaymentMethods(paymentService, model.PaymentMethods, PaymentType.Letters);


            foreach (var partnerInvoice in invoices)
            {
                model.PartnerInvoices.Add(new PartnerInvoiceViewModel()
                {
                    Amount = partnerInvoice.Price.PriceExVat,
                    Currency = partnerInvoice.Price.CurrencyCode.ToString(),
                    PdfUrl = partnerInvoice.PdfUrl,
                    CustomerName = partnerInvoice.CustomerName,
                    InvoiceDate = partnerInvoice.InvoiceDate,
                    OrderId = partnerInvoice.OrderId,
                    Id = partnerInvoice.Id,
                    Status = partnerInvoice.LetterAmazerStatus,
                    CustomerAddress = partnerInvoice.CustomerAddress,
                    CustomerCity = partnerInvoice.CustomerCity,
                    CustomerCountry = partnerInvoice.CustomerCountry,
                    CustomerCounty = partnerInvoice.CustomerCounty,
                    DownloadPdfLink = "/partner/GetEconomicInvoice?pdfUrl=" + partnerInvoice.PdfUrl +"&token=" + token
                });
            }

            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Economic(PartnerInvoiceOverviewViewModel model)
        {
            // TODO: would someone please clean up this 100000 line long code? at least make subroutines?
            var customerId = model.UserId;
            Checkout checkout = new Checkout()
            {
                UserId = customerId,
                PaymentMethodId = model.PaymentMethodId
            };


            foreach (var selectedInvoice in model.SelectedInvoices[0].Split(';'))
            {
                var deliveryCountryId = int.Parse(model.SelectedCountry);
                var invoice = economicInvoiceService.GetPartnerInvoiceById(model.Token, selectedInvoice);

                checkout.PartnerTransactions.Add(new PartnerTransaction()
                  {
                      CustomerId = customerId,
                      PartnerId = 1,
                      ValueId = int.Parse(invoice.Id)
                  });

                byte[] data=economicInvoiceService.GetEconomicPdfBytes(model.AccessToken, invoice.PdfUrl);
                
                var uploadFile = fileService.Create(data, Guid.NewGuid().ToString(), FileUploadMode.Temporarily);
                var priceInfo = priceService.GetPricesFromFiles(new[] { uploadFile }, customerId, deliveryCountryId);

                    var officeProduct = officeProductService.GetOfficeProductById(priceInfo.OfficeProductId);

                    var t = new CheckoutLine()
                    {
                        OfficeProductId = priceInfo.OfficeProductId,
                        Letter = new Letter()
                        {
                            ToAddress = new AddressInfo()
                            {
                                Country = countryService.GetCountryById(deliveryCountryId)
                            },
                            LetterContent = new LetterContent()
                            {
                                Path = uploadFile,
                                Content = data
                            },
                            OfficeId = officeProduct.OfficeId
                        }
                    };
                    checkout.CheckoutLines.Add(t);
                
            }

            var order = checkoutService.ConvertCheckout(checkout);
            var updated_order = orderService.Create(order);

            string redirectUrl = paymentService.Process(updated_order);

            if (string.IsNullOrEmpty(redirectUrl))
            {
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                dashboardViewModel.DashboardStatus = DashboardStatus.SendLetter;  
                return RedirectToAction("Index", "User", dashboardViewModel);
            }
            return Redirect(redirectUrl);
        }

        public ActionResult GetEconomicInvoice(string token, string pdfUrl)
        {
            var data = economicInvoiceService.GetEconomicPdfBytes(token, pdfUrl);
            return File(data, "application/pdf");
        }
    }
}
