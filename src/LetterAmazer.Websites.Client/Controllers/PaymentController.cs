using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using iTextSharp.xmp.impl;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Websites.Client.ViewModels.Payment;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class PaymentController : Controller
    {
        private IOrderService orderService;
        public PaymentController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        //
        // GET: /Payment/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Invoice(Guid id)
        {
            var order = orderService.GetOrderById(id);

            var invoiceModel = new InvoiceViewModel()
            {
                DateCreated = order.DateCreated,
                Id = order.Id,
                CompanyInfo = new InvoiceAddressViewModel()
                {
                    Address = LetterAmazer.Business.Utils.Helpers.Constants.Texts.PracticalInformation.Street,
                    Company = LetterAmazer.Business.Utils.Helpers.Constants.Texts.PracticalInformation.CompanyName,
                    VatNumber = LetterAmazer.Business.Utils.Helpers.Constants.Texts.PracticalInformation.Cvr,
                    ZipCode = LetterAmazer.Business.Utils.Helpers.Constants.Texts.PracticalInformation.PostalNr,
                    City = LetterAmazer.Business.Utils.Helpers.Constants.Texts.PracticalInformation.City,
                    Country = LetterAmazer.Business.Utils.Helpers.Constants.Texts.PracticalInformation.Country,
                    Name = LetterAmazer.Business.Utils.Helpers.Constants.Texts.PracticalInformation.AttPerson,
                },
                ReceiverInfo = new InvoiceAddressViewModel()
                {
                    Address = order.Customer.CustomerInfo.Address1,
                    City = order.Customer.CustomerInfo.City,
                    Company = order.Customer.CustomerInfo.Organisation,
                    Name = order.Customer.CustomerInfo.FirstName + " " + order.Customer.CustomerInfo.LastName,
                    VatNumber = order.Customer.CustomerInfo.VatNr,
                    ZipCode  = order.Customer.CustomerInfo.Zipcode
                }
            };

            foreach (var letterLine in order.OrderLines.Where(c=>c.ProductType != ProductType.Payment))
            {
                invoiceModel.Lines.Add(new InvoiceLineViewModel()
                {
                    Price = letterLine.Cost,
                    Quantity = letterLine.Quantity,
                    Title = letterLine.ProductType.ToString()
                });
            }

            return View(invoiceModel);
        }
    }
}
