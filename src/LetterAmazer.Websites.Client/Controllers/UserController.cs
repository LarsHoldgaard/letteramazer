using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;
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
        private IPriceService priceService;
        

        public UserController(IOrderService orderService, IPaymentService paymentService,
            ILetterService letterService, ICouponService couponService, ICountryService countryService, IPriceService priceService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.letterService = letterService;
            this.couponService = couponService;
            this.countryService = countryService;
            this.priceService = priceService;
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

                Order order = new Order();


                AddressInfo addressInfo = new AddressInfo();
                addressInfo.Address1 = model.DestinationAddress;
                addressInfo.FirstName = model.RecipientName;
                addressInfo.City = model.DestinationCity;
                addressInfo.Country = countryService.GetCountryBySpecificaiton(
                    new CountrySpecification() { CountryCode = model.DestinationCountryCode }).FirstOrDefault();
                addressInfo.PostalCode = model.ZipCode;


                Customer customer = new Customer();
                customer.Email = model.Email;
                customer.Phone = model.Phone;
                customer.CustomerInfo = addressInfo;
                order.Customer = customer;


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

                if (model.UseUploadFile)
                {
                    logger.DebugFormat("upload file key: {0}", model.UploadFile);
                    letter.LetterContent.Path = model.UploadFile;
                }
                else
                {
                    string tempKeyName = string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month,
                        Guid.NewGuid().ToString());
                    string tempPath = PathHelper.GetAbsoluteFile(tempKeyName);

                    var convertedText = HelperMethods.Utf8FixString(model.WriteContent);
                    PdfHelper.ConvertToPdf(tempPath, convertedText);
                    letter.LetterContent.Path = tempKeyName;
                    letter.LetterContent.WrittenContent = model.WriteContent;
                }
                if (System.IO.File.Exists(PathHelper.GetAbsoluteFile(letter.LetterContent.Path)))
                {
                    letter.LetterContent.Content =
                        System.IO.File.ReadAllBytes(PathHelper.GetAbsoluteFile(letter.LetterContent.Path));
                }

                var price = priceService.GetPriceByAddress(addressInfo, letter.LetterContent.PageCount);
                letter.OfficeProductId = price.OfficeProductId;

                Coupon coupon = null;
                if (!string.IsNullOrEmpty(model.VoucherCode))
                {
                    var voucher = couponService.GetCouponBySpecification(new CouponSpecification()
                    {
                        Code = model.VoucherCode
                    });
                    if (voucher != null && voucher.Any())
                    {
                        coupon = (Coupon)voucher.FirstOrDefault();
                    }
                }


                order.OrderLines.Add(new OrderLine()
                {
                    ProductType = ProductType.Order,
                    BaseProduct = letter,
                    Cost = priceService.GetPriceByLetter(letter).PriceExVat
                });

                var rest = addCouponlines(price, coupon, order);

                if (rest > 0)
                {
                    order.OrderLines.Add(new OrderLine()
                    {
                        ProductType = ProductType.Payment,
                        Cost = rest,
                        PaymentMethod = paymentService.GetPaymentMethodById(1) // Paypal
                    });
                }

                var storedOrder = orderService.Create(order);

                string redirectUrl = paymentService.Process(storedOrder);

                if (string.IsNullOrEmpty(redirectUrl))
                {
                    return RedirectToAction("Confirmation", "SingleLetter");
                }

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                logger.Error(ex.InnerException);
                ModelState.AddModelError("Business", ex.Message);
            }

            return RedirectToActionWithError("Index", model);
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
            model.Credit = SessionHelper.Customer.Credit;
            model.CreditLimit = SessionHelper.Customer.CreditLimit;

            var possiblePaymentMethods = paymentService.GetPaymentMethodsBySpecification(new PaymentMethodSpecification()
            {
                CustomerId = SessionHelper.Customer.Id
            });

            model.PaymentMethods = getPaymentmethodViewModels(possiblePaymentMethods);

            return View(model);
        }




        [HttpPost]
        public ActionResult Credits(CreditsViewModel model)
        {
            var selectedPaymentMethod = paymentService.GetPaymentMethodById(model.SelectedPaymentMethod);
            var credit = new Credit();

            var creditLine = new OrderLine()
            {
                Quantity = model.PurchaseAmount,
                Cost = model.PurchaseAmount,
                ProductType = ProductType.Credit,
                BaseProduct = credit,
                
            };

            var paymentLine = new OrderLine()
            {
                PaymentMethod = selectedPaymentMethod,
                ProductType = ProductType.Payment,
                Cost = model.PurchaseAmount
            };

            Order order = new Order();
            order.Price = model.PurchaseAmount;
            order.Customer = SessionHelper.Customer;
            order.OrderLines.Add(creditLine);
            order.OrderLines.Add(paymentLine);

            var placed_order = orderService.Create(order);
            string redirectUrl = paymentService.Process(placed_order);

            if (string.IsNullOrEmpty(redirectUrl))
            {
                return RedirectToAction("Confirmation","SingleLetter");
            }
            return Redirect(redirectUrl);
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

        private List<OrderViewModel> getOrderViewModel(IEnumerable<Order> orders)
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

        private List<OrderLineViewModel> getOrderLineViewModel(IEnumerable<OrderLine> orderLines)
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

        private IEnumerable<PaymentMethodViewModel> getPaymentmethodViewModels(IEnumerable<PaymentMethods> paymentMethods)
        {
            return paymentMethods.Select(paymentMethodse => new PaymentMethodViewModel()
            {
                Id = paymentMethodse.Id,
                Name = paymentMethodse.Name
            }).ToList();
        }

        private decimal addCouponlines(Price price, Coupon coupon, Order order)
        {
            decimal rest = price.PriceExVat;
            if (coupon != null)
            {
                decimal chargeCoupon = 0.0m;
                if (rest > coupon.CouponValueLeft)
                {
                    chargeCoupon = coupon.CouponValueLeft;
                }
                else
                {
                    chargeCoupon = rest;
                }

                order.OrderLines.Add(new OrderLine()
                {
                    ProductType = ProductType.Payment,
                    Cost = chargeCoupon,
                    PaymentMethod = paymentService.GetPaymentMethodById(3), // coupon                        
                    CouponId = coupon.Id
                });

                rest -= coupon.CouponValueLeft;
            }
            return rest;
        }


        #endregion
    }
}
