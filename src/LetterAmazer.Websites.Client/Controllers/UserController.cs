﻿using System.Linq;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Domain.Session;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Business.Services.Services.PaymentMethods.Implementations;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.Attributes;
using LetterAmazer.Websites.Client.ViewModels;
using LetterAmazer.Websites.Client.Extensions;
using log4net;
using System;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(UserController));
        private IOrderService orderService;
        private ISessionService sessionService;
        private IPaymentService paymentService;
        private ILetterService letterService;
        private ICouponService couponService;
        private ICountryService countryService;

        public UserController(IOrderService orderService, IPaymentService paymentService, 
            ILetterService letterService, ICouponService couponService, ISessionService sessionService, ICountryService countryService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.letterService = letterService;
            this.couponService = couponService;
            this.sessionService = sessionService;
            this.countryService = countryService;
        }

        public ActionResult Index(int? page, ProfileViewModel model)
        {
            var orders = orderService.GetOrderBySpecification(new OrderSpecification()
            {
                UserId = sessionService.Customer.Id,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            });

            model.Orders = orders;
            model.Customer = sessionService.Customer;

            return View(model);
        }

        [HttpGet]
        public ActionResult SendALetter()
        {
            CreateSingleLetterModel model = new CreateSingleLetterModel();
            model.Email = sessionService.Customer.Email;
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SendALetter(CreateSingleLetterModel model)
        {
            try
            {
                ValidateInput();

                Order order = new Order();
                order.Customer = sessionService.Customer;
                
                AddressInfo addressInfo = new AddressInfo();
                addressInfo.Address1 = model.DestinationAddress;
                addressInfo.FirstName = model.RecipientName;
                addressInfo.City = model.DestinationCity;
                addressInfo.Country = countryService.GetCountryBySpecificaiton(
                    new CountrySpecification() 
                        {CountryCode = model.DestinationCountryCode }).FirstOrDefault();// TODO: Fix country
                addressInfo.PostalCode = model.ZipCode;

                LetterDe letterDetail = new LetterDetail();
                letterDetail.Color = (int)LetterColor.Color;
                letterDetail.LetterTreatment = (int)LetterProcessing.Dull;
                letterDetail.LetterWeight = (int) LetterPaperWeight.Eight;
                letterDetail.Size = (int) LetterSize.A4;

                OrderItem orderItem = new OrderItem();

                Letter letter = new Letter();
                letter.LetterStatus = LetterStatus.Created;
                letter.LetterDetail = letterDetail;
                letter.CustomerId = SecurityUtility.CurrentUser.Id;
                letter.Customer = SecurityUtility.CurrentUser;
                letter.ToAddress = addressInfo;

                if (model.UseUploadFile)
                {
                    logger.DebugFormat("upload file key: {0}", model.UploadFile);
                    letter.LetterContent.Path = model.UploadFile;
                }
                else
                {
                    string tempKeyName = string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
                    string tempPath = GetAbsoluteFile(tempKeyName);
                    PdfHelper m = new PdfHelper();
                    var convertedText = HelperMethods.Utf8FixString(model.WriteContent);
                    m.ConvertToPdf(tempPath, convertedText);
                    letter.LetterContent.Path = tempKeyName;
                    letter.LetterContent.WrittenContent = model.WriteContent;
                }
                if (System.IO.File.Exists(GetAbsoluteFile(letter.LetterContent.Path)))
                {
                    letter.LetterContent.Content = System.IO.File.ReadAllBytes(GetAbsoluteFile(letter.LetterContent.Path));
                }

                orderItem.Letter = letter;
                orderItem.Order = order;
                order.OrderItems.Add(orderItem);

                OrderContext orderContext = new OrderContext();
                orderContext.Order = order;
                orderContext.SignUpNewsletter = model.SignUpNewsletter;
                orderContext.CurrentCulture = RouteData.Values["culture"].ToString();
                string redirectUrl = orderService.CreateOrder(orderContext);
                
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                logger.Error(ex.InnerException);
                ModelState.AddModelError("Business", ex.Message);
            }

            return RedirectToActionWithError("SendALetter", model);
        }

        [HttpGet, AutoErrorRecovery]
        public ActionResult Delete(int id)
        {
            Order order = orderService.GetOrderById(id);
            OrderDetailViewModel model = new OrderDetailViewModel();
            model.Order = order;
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string submitAction)
        {
            try
            {
                orderService.DeleteOrder(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ModelState.AddBusinessError(ex.Message);
            }

            Order order = orderService.GetOrderById(id);
            OrderDetailViewModel model = new OrderDetailViewModel();
            model.Order = order;
            return RedirectToActionWithError("Delete", model, new { id = id });
        }

        public ActionResult Details(int id)
        {
            Order order = orderService.GetOrderById(id);
            OrderDetailViewModel model = new OrderDetailViewModel();
            model.Order = order;
            return View(model);
        }

        public FileResult Download(int id)
        {
            Letter letter = letterService.GetLetterById(id);
            return File(letter.LetterContent.Content, "application/pdf", id + ".pdf");
        }

        [HttpGet]
        public ActionResult Credits()
        {
            CreditsViewModel model = new CreditsViewModel();
            model.Credits = SecurityUtility.CurrentUser.Credit;
            model.CreditLimit = SecurityUtility.CurrentUser.CreditLimit;
            return View(model);
        }

        [HttpPost]
        public ActionResult Credits(CreditsViewModel model)
        {
            string returnUrl = orderService.AddFunds(SecurityUtility.CurrentUser.Id, model.Funds);
            return Redirect(returnUrl);
        }

        private string GetUploadFileName(string uploadFilename)
        {
            return string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
        }

        private string GetAbsoluteFile(string filename)
        {
            return Server.MapPath(letterService.GetRelativeLetterStoragePath().TrimEnd('/') + "/" + filename);
        }
    }
}
