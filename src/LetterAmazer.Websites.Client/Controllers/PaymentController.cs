using System;
using System.Collections.Generic;
using System.Configuration;
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


        public ActionResult Epay(int id)
        {
            var acceptUrl = ConfigurationManager.AppSettings["LetterAmazer.Payment.Successful"];
            var callbackUrl = ConfigurationManager.AppSettings["LetterAmazer.Payment.Epay.CallbackUrl"];
            var declineUrl = ConfigurationManager.AppSettings["LetterAmazer.Payment.Decline"];
            var googleTracker = ConfigurationManager.AppSettings["LetterAmazer.Settings.Analytics"];
            var merchantNumber = ConfigurationManager.AppSettings["LetterAmazer.Payment.Epay.MerchantNumber"];

            var order = orderService.GetOrderById(id);

            var epayModel = new EpayViewModel()
            {
                OrderId = id.ToString(),
                Amount = order.Price.Total,
                AcceptUrl = acceptUrl,
                CallbackUrl = callbackUrl,
                DeclineUrl = declineUrl,
                GoogleTracker = googleTracker,
                MerchantNumber = merchantNumber
            };

            return View(epayModel);
        }

        public ActionResult Invoice(Guid id)
        {
            var invoice = invoiceService.GetInvoiceById(id);

            var invoiceModel = new InvoiceViewModel()
            {
                DateCreated = invoice.DateCreated,
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                VatPercentage = invoice.PriceVatPercentage * 100,
                Total = invoice.PriceTotal,
                Status = invoice.InvoiceStatus.ToString(),
                TotalExVat = invoice.PriceExVat,
                VatTotal = invoice.PriceVat,
                CompanyInfo = new InvoiceAddressViewModel()
                {
                    Address = invoice.InvoiceInfo.Address1,
                    City = invoice.InvoiceInfo.City,
                    ZipCode = invoice.InvoiceInfo.Zipcode,
                    VatNumber = invoice.InvoiceInfo.VatNr,
                    Company = invoice.InvoiceInfo.Organisation,
                    Name = invoice.InvoiceInfo.AttPerson,
                    Country = invoice.InvoiceInfo.Country.Name,
                    Cvr = invoice.InvoiceInfo.VatNr
                },
                ReceiverInfo = new InvoiceAddressViewModel()
                {
                    Address = invoice.ReceiverInfo.Address1,
                    City = invoice.ReceiverInfo.City,
                    ZipCode = invoice.ReceiverInfo.Zipcode,
                    VatNumber = invoice.ReceiverInfo.VatNr,
                    Company = invoice.ReceiverInfo.Organisation,
                    Name = invoice.ReceiverInfo.AttPerson,
                    Country = invoice.ReceiverInfo.Country.Name,
                    Cvr = invoice.ReceiverInfo.VatNr
                },
                InvoicePaymentMessage = invoice.InvoicePaymentMessage
            };

            foreach (var invoiceLine in invoice.InvoiceLines)
            {
                invoiceModel.Lines.Add(new InvoiceLineViewModel()
                {
                    Price = invoiceLine.PriceExVat,
                    Quantity = invoiceLine.Quantity,
                    Title = invoiceLine.Description,
                });
            }

            return View(invoiceModel);
        }


    }
}
