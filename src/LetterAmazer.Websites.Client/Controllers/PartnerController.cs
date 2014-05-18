using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Checkout;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Services.Partners.Invoice;
using LetterAmazer.Business.Utils.Helpers;
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

        public PartnerController(IOrderService orderService, IPaymentService paymentService, ICheckoutService checkoutService,
            IOfficeProductService officeProductService)
        {
            this.economicInvoiceService = new EconomicInvoiceService();
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.checkoutService = checkoutService;
            this.officeProductService = officeProductService;
        }

        public ActionResult Economic()
        {
            var model = new PartnerInvoiceOverviewViewModel();

            var invoices = economicInvoiceService.GetPartnerInvoiceBySpecification(new PartnerInvoiceSpecification()
            {
                From = model.From,
                To = model.To
            });

            foreach (var partnerInvoice in invoices)
            {
                model.PartnerInvoices.Add(new PartnerInvoiceViewModel()
                {
                    Amount = partnerInvoice.Price.PriceExVat,
                    Currency = partnerInvoice.Price.CurrencyCode.ToString(),
                    PdfUrl = partnerInvoice.PdfUrl,
                    CustomerName = partnerInvoice.CustomerName,
                    InvoiceDate = partnerInvoice.InvoiceDate,
                    OrderId = partnerInvoice.Id
                });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Economic(PartnerInvoiceOverviewViewModel model)
        {
            Checkout checkout = new Checkout()
            {
                UserId = SessionHelper.Customer != null ? SessionHelper.Customer.Id : 0,
                Email = SessionHelper.Customer.Email,
                PaymentMethodId = 2
            };

            foreach (var selectedInvoice in model.SelectedInvoices)
            {
                var invoice = economicInvoiceService.GetPartnerInvoiceById(selectedInvoice);
                int i = 0;
                //var priceInfo = GetPriceFromFile(uploadFile,model.DestinationCountry);
                //var officeProduct = officeProductService.GetOfficeProductById(priceInfo.OfficeProductId);

                //var t = new CheckoutLine()
                //{
                //    OfficeProductId = priceInfo.OfficeProductId,
                //    Letter = new Letter()
                //    {
                //        ToAddress = new AddressInfo()
                //        {
                //            Country = countryService.GetCountryById(model.DestinationCountry)
                //        },
                //        LetterContent = new LetterContent()
                //        {
                //            Path = uploadFile
                //        },
                //        OfficeId = officeProduct.OfficeId
                //    }
                //}
                //checkout.Letters.Add(t);
            }

            //var updated_order = orderService.Create(order);

            //string redirectUrl = paymentService.Process(updated_order);
            throw new NotImplementedException();
        }


    }
}
