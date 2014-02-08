using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Model;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Business.Services.Services.LetterContent;
using LetterAmazer.Business.Services.Services.PaymentMethods;
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
        private IPaymentService paymentService;
        private ILetterService letterService;
        private ICouponService couponService;
        public UserController(IOrderService orderService, IPaymentService paymentService, 
            ILetterService letterService, ICouponService couponService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.letterService = letterService;
            this.couponService = couponService;
        }

        public ActionResult Index(int? page, ProfileViewModel model)
        {
            OrderCriteria criteria = new OrderCriteria();
            criteria.PageIndex = page.HasValue ? page.Value - 1 : 0;
            criteria.PageSize = 20;
            criteria.OrderBy.Add(OrderBy.Desc("DateCreated"));
            criteria.CustomerId = SecurityUtility.CurrentUser.Id;
            criteria.From = model.FromDate;
            criteria.To = model.ToDate;
            criteria.OrderType = OrderType.SendLetters;

            PaginatedResult<Order> orders = orderService.GetOrders(criteria);

            model.Orders = new PaginatedInfo<Order>(criteria.PageIndex, criteria.PageSize, orders);
            model.Customer = SecurityUtility.CurrentUser;

            return View(model);
        }

        [HttpGet]
        public ActionResult SendALetter()
        {
            CreateSingleLetterModel model = new CreateSingleLetterModel();
            model.Email = SecurityUtility.CurrentUser.Email;
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SendALetter(CreateSingleLetterModel model)
        {
            try
            {
                ValidateInput();

                Order order = new Order();
                order.Email = model.Email;
                order.Phone = model.Phone;
                order.PaymentMethod = CreditsMethod.NAME;
                order.CouponCode = model.VoucherCode;
                order.Customer = SecurityUtility.CurrentUser;
                order.CustomerId = SecurityUtility.CurrentUser.Id;

                AddressInfo addressInfo = new AddressInfo();
                addressInfo.Address1 = model.DestinationAddress;
                addressInfo.FirstName = model.RecipientName;
                addressInfo.City = model.DestinationCity;
                //addressInfo.Country = model.DestinationCountry;// TODO: Fix country
                addressInfo.Postal = model.ZipCode;

                LetterDetail letterDetail = new LetterDetail();
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
                    PdfManager m = new PdfManager();
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
