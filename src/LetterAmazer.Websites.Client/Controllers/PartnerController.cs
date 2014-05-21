using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor.Diagnostics.Extensions;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Checkout;
using LetterAmazer.Business.Services.Domain.Countries;
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
        public PartnerController(IOrderService orderService, IPaymentService paymentService, ICheckoutService checkoutService,
            IOfficeProductService officeProductService, IPriceService priceService, ICountryService countryService, IFileService fileService, EconomicInvoiceService economicInvoiceService)
        {
            this.economicInvoiceService = economicInvoiceService;
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.checkoutService = checkoutService;
            this.officeProductService = officeProductService;
            this.priceService = priceService;
            this.countryService = countryService;
            this.fileService = fileService;
        }

        public ActionResult Economic()
        {
            var model = new PartnerInvoiceOverviewViewModel();

            var invoices = economicInvoiceService.GetPartnerInvoiceBySpecification(new PartnerInvoiceSpecification()
            {
                From = model.From,
                To = model.To
            });

            new Helper().FillCountries(model.Countries);

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
                    CustomerCounty = partnerInvoice.CustomerCounty
                });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Economic(PartnerInvoiceOverviewViewModel model)
        {
            var customerId = SessionHelper.Customer != null ? SessionHelper.Customer.Id : 0;
            Checkout checkout = new Checkout()
            {
                UserId = customerId,
                Email = SessionHelper.Customer.Email,
                PaymentMethodId = 2
            };


            foreach (var selectedInvoice in model.SelectedInvoices[0].Split(';'))
            {
                var deliveryCountryId = int.Parse(model.SelectedCountry);
                var invoice = economicInvoiceService.GetPartnerInvoiceById(selectedInvoice);

                checkout.PartnerTransactions.Add(new PartnerTransaction()
                  {
                      CustomerId = customerId,
                      PartnerId = 1,
                      ValueId = int.Parse(invoice.Id)
                  });

                string fileKey = string.Empty;
                using (var client = new WebClient())
                {
                    var data = client.DownloadData(invoice.PdfUrl);
                    fileKey = fileService.Create(data, Business.Services.Utils.Helpers.GetUploadDateString(Guid.NewGuid().ToString()));
                }

                var priceInfo = priceService.GetPricesFromFiles(new[] { fileKey }, customerId, deliveryCountryId);  // TODO: wtf?

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
                            Path = fileKey
                        },
                        OfficeId = officeProduct.OfficeId
                    }
                };
                checkout.Letters.Add(t);
            }

            var order = checkoutService.ConvertCheckout(checkout);
            var updated_order = orderService.Create(order);

            string redirectUrl = paymentService.Process(updated_order);

            if (string.IsNullOrEmpty(redirectUrl))
            {
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                return RedirectToAction("Index", "User", dashboardViewModel);
            }
            return Redirect(redirectUrl);
        }


    }
}
