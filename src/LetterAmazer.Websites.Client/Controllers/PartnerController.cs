using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Business.Services.Services.Partners.Invoice;
using LetterAmazer.Websites.Client.ViewModels.Partner;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class PartnerController : Controller
    {
        private EconomicInvoiceService economicInvoiceService;

        public PartnerController()
        {
            this.economicInvoiceService = new EconomicInvoiceService();
        }

        public ActionResult Economic()
        {
            var model = new PartnerInvoiceOverviewViewModel();

            var invoices = economicInvoiceService.GetBySpecification(new PartnerInvoiceSpecification()
            {
                  From = model.From,
                  To = model.To
            });

            foreach (var partnerInvoice in invoices)
            {
                model.PartnerInvoices.Add(new PartnerInvoiceViewModel()
                {
                    Id = partnerInvoice.Id,
                    DateCreated = partnerInvoice.DateCreated,
                    PdfUrl = partnerInvoice.PdfUrl
                });
            }

            return View(model);
        }

    }
}
