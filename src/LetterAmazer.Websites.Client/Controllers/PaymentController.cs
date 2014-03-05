using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using iTextSharp.xmp.impl;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Websites.Client.ViewModels.Payment;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class PaymentController : Controller
    {
        private IOrderService orderService;
        private IInvoiceService invoiceService;
        public PaymentController(IOrderService orderService, IInvoiceService invoiceService)
        {
            this.orderService = orderService;
            this.invoiceService = invoiceService;
        }

        //
        // GET: /Payment/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Invoice(Guid id)
        {
            var invoice = invoiceService.GetInvoiceById(id);

            var invoiceModel = new InvoiceViewModel()
            {
                DateCreated = invoice.DateCreated,
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                VatPercentage =invoice.PriceVatPercentage,
                Total = invoice.PriceTotal,
                TotalExVat = invoice.PriceExVat,
                CompanyInfo = new InvoiceAddressViewModel()
                {
                Address = invoice.InvoiceInfo.Address1,
                   City = invoice.InvoiceInfo.City,
                   ZipCode = invoice.InvoiceInfo.Zipcode,
                   VatNumber = invoice.InvoiceInfo.VatNr,
                   Company = invoice.InvoiceInfo.Organisation,
                   Name = invoice.InvoiceInfo.AttPerson,
                   Country = invoice.InvoiceInfo.Country.Name
                },
                ReceiverInfo = new InvoiceAddressViewModel()
                {
                    Address = invoice.ReceiverInfo.Address1,
                    City = invoice.ReceiverInfo.City,
                    ZipCode = invoice.ReceiverInfo.Zipcode,
                    VatNumber = invoice.ReceiverInfo.VatNr,
                    Company = invoice.ReceiverInfo.Organisation,
                    Name = invoice.ReceiverInfo.AttPerson,
                    Country = invoice.ReceiverInfo.Country.Name
                }
            };

            foreach (var invoiceLine in invoice.InvoiceLines)
            {
                invoiceModel.Lines.Add(new InvoiceLineViewModel()
                {
                    Price = invoiceLine.PriceExVat,
                    Quantity = invoiceLine.Quantity,
                    Title = invoiceLine.Description
                });
            }

            return View(invoiceModel);
        }
    }
}
