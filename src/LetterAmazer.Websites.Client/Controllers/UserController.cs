using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.Attributes;
using LetterAmazer.Websites.Client.ViewModels;
using LetterAmazer.Websites.Client.Extensions;
using log4net;
using System;
using System.Web.Mvc;
using ProductType = LetterAmazer.Business.Services.Domain.Products.ProductType;

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
        private ICountryService countryService;


        public UserController(IOrderService orderService, IPaymentService paymentService,
            ILetterService letterService, ICouponService couponService, ICountryService countryService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.letterService = letterService;
            this.couponService = couponService;
            this.countryService = countryService;
        }

        public ActionResult Index(int? page, ProfileViewModel model)
        {
            var orders = orderService.GetOrderBySpecification(new OrderSpecification()
            {
                UserId = SessionHelper.Customer.Id,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            });

            model.Orders = getOrderViewModel(orders);
            model.Customer = SessionHelper.Customer;

            return View(model);
        }

        [HttpGet]
        public ActionResult SendALetter()
        {
            CreateSingleLetterModel model = new CreateSingleLetterModel();
            model.Email = SessionHelper.Customer.Email;
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SendALetter(CreateSingleLetterModel model)
        {
            try
            {
                ValidateInput();

                LetterContent letterContent = new LetterContent();

                if (model.UseUploadFile)
                {
                    logger.DebugFormat("upload file key: {0}", model.UploadFile);
                    letterContent.Path = model.UploadFile;
                }
                else
                {
                    string tempKeyName = string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
                    string tempPath = GetAbsoluteFile(tempKeyName);

                    var convertedText = HelperMethods.Utf8FixString(model.WriteContent);
                    PdfHelper.ConvertToPdf(tempPath, convertedText);
                    letterContent.Path = tempKeyName;
                    letterContent.WrittenContent = model.WriteContent;
                }
                if (System.IO.File.Exists(GetAbsoluteFile(letterContent.Path)))
                {
                    letterContent.Content = System.IO.File.ReadAllBytes(GetAbsoluteFile(letterContent.Path));
                }



               AddressInfo addressInfo = new AddressInfo();
                addressInfo.Address1 = model.DestinationAddress;
                addressInfo.FirstName = model.RecipientName;
                addressInfo.City = model.DestinationCity;
                addressInfo.Country = countryService.GetCountryBySpecificaiton(
                    new CountrySpecification() { CountryCode = model.DestinationCountryCode }).FirstOrDefault();
                addressInfo.PostalCode = model.ZipCode;

                LetterDetails letterDetail = new LetterDetails()
                {
                    LetterColor = LetterColor.Color,
                    LetterPaperWeight = LetterPaperWeight.Eight,
                    LetterProcessing = LetterProcessing.Dull,
                    LetterSize = LetterSize.A4,
                    LetterType = LetterType.Pres
                };

                Letter letter = new Letter()
                {
                    LetterDetails = letterDetail,
                    ToAddress = addressInfo,
                    LetterStatus = LetterStatus.Created,
                };

              
                OrderLine orderItem = new OrderLine()
                {
                    BaseProduct = letter,
                    ProductType = ProductType.Order,
                    Quantity = 1
                };

                Order order = new Order()
                {
                    Customer = SessionHelper.Customer,
                    OrderStatus = OrderStatus.Created
                };
                order.OrderLines.Add(orderItem);

                var url = paymentService.Process(order);

                return Redirect(url);
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
                var delete_order = orderService.GetOrderById(id);
                orderService.Delete(delete_order);
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
            model.Credits = SessionHelper.Customer.Credit;
            model.CreditLimit = SessionHelper.Customer.CreditLimit;
            return View(model);
        }

        [HttpPost]
        public ActionResult Credits(CreditsViewModel model)
        {
            string url = "";
            //string returnUrl = orderService.AddFunds(SecurityUtility.CurrentUser.Id, model.Funds);
            return Redirect(url);
        }

        #region Private helpers

        private string GetUploadFileName(string uploadFilename)
        {
            return string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
        }

        private string GetAbsoluteFile(string filename)
        {
            return string.Empty;
            //return Server.MapPath(letterService.GetRelativeLetterStoragePath().TrimEnd('/') + "/" + filename);
        }

        private List<OrderViewModel> getOrderViewModel(List<Order> orders)
        {
            List<OrderViewModel> ordersViewModels = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                OrderViewModel viewModel = new OrderViewModel()
                {
                    OrderLines = getOrderLineViewModel(order.OrderLines),
                    DateCreated = order.DateCreated,
                    OrderStatus = order.OrderStatus,
                    Id = order.Id,
                    Price = order.Price
                };

                ordersViewModels.Add(viewModel);
            }
            return ordersViewModels;
        }

        private List<OrderLineViewModel> getOrderLineViewModel(List<OrderLine> orderLines)
        {
            List<OrderLineViewModel> lines =new List<OrderLineViewModel>();
            foreach (var orderline in orderLines)
            {
                lines.Add(new OrderLineViewModel()
                {
                    Quantity = orderline.Quantity,
                    OrderLineProductViewModel = getOrderLineProductViewModel((Letter)orderline.BaseProduct)
                });
            }
            return lines;
        }

        private OrderLineProductViewModel getOrderLineProductViewModel(Letter letter)
        {
            return new OrderLineProductViewModel()
            {
                
            };
        }

        #endregion
    }
}
