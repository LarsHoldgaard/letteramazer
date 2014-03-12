using System.Data.Entity.Infrastructure;
using System.Linq;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.PriceUpdater;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Exceptions;
using log4net;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using LetterAmazer.Websites.Client.ViewModels;
using LetterAmazer.Business.Utils.Helpers;
using ProductType = LetterAmazer.Business.Services.Domain.Products.ProductType;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class SingleLetterController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SingleLetterController));

        private IOrderService orderService;

        private IPaymentService paymentService;
        private ICouponService couponService;
        private ICountryService countryService;
        private IPriceService priceService;
        private ICustomerService customerService;
        private IPriceUpdater priceUpdater;
        private IDeliveryJobService deliveryJobService;
        
        public SingleLetterController(IOrderService orderService, IPaymentService paymentService,
            ICouponService couponService, ICountryService countryService, IPriceService priceService,
            ICustomerService customerService,IPriceUpdater priceUpdater, IDeliveryJobService deliveryJobService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.couponService = couponService;
            this.countryService = countryService;
            this.priceService = priceService;
            this.customerService = customerService;
            this.priceUpdater = priceUpdater;
            this.deliveryJobService = deliveryJobService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (SessionHelper.Customer != null)
            {
                if (SessionHelper.Customer.CreditsLeft > 0.0m)
                {
                    return RedirectToAction("SendALetter", "User");
                }
                else
                {
                    return RedirectToAction("Credits", "User");
                }
            }
                
            CreateSingleLetterModel model = new CreateSingleLetterModel();

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(CreateSingleLetterModel model)
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
                    new CountrySpecification() {CountryCode = model.DestinationCountryCode}).FirstOrDefault();
                addressInfo.Zipcode = model.ZipCode;
                addressInfo.State = model.DestinationState;

                Customer customer = null;
                var existingCustomer = customerService.GetCustomerBySpecification(new CustomerSpecification()
                {
                    Email = model.Email
                }).FirstOrDefault();

                if (existingCustomer == null)
                {
                    Customer newCustomer = new Customer();
                    newCustomer.Email = model.Email;
                    newCustomer.Phone = model.Phone;

                    customer = customerService.Create(newCustomer);
                }
                else
                {
                    customer = existingCustomer;
                }


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
                    ToAddress = addressInfo

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
                        coupon = (Coupon) voucher.FirstOrDefault();
                    }
                }


                order.OrderLines.Add(new OrderLine()
                {
                    ProductType = ProductType.Letter,
                    BaseProduct = letter,
                    Price = new Price()
                    {
                        PriceExVat = priceService.GetPriceByLetter(letter).Total
                    }
                });

                var rest = addCouponlines(price, coupon, order);

                if (rest > 0)
                {
                    order.OrderLines.Add(new OrderLine()
                    {
                        ProductType = ProductType.Payment,
                        PaymentMethodId = 1, // PayPal,
                        Price = new Price()
                        {
                            PriceExVat = rest
                        }
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

        private decimal addCouponlines(Price price, Coupon coupon, Order order)
        {
            decimal rest = price.Total;
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
                    PaymentMethodId = 3, // coupon                        
                    CouponId = coupon.Id,
                    Price = new Price()
                    {
                        PriceExVat = chargeCoupon
                    }
                });

                rest -= coupon.CouponValueLeft;
            }
            return rest;
        }

        public ActionResult DropZone()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                HttpPostedFileBase uploadFile = Request.Files[0];
                string keyName = GetUploadFileName(uploadFile.FileName);
                string filename = PathHelper.GetAbsoluteFile(keyName);
                string path = Path.GetDirectoryName(filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                uploadFile.SaveAs(filename);
                return Json(new
                {
                    status = "success",
                    key = keyName
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetPrice(bool usePdf, string uploadFileKey, string content, string address, string postal, string city, string state,string country)
        {
            try
            {
                Letter letter = new Letter();
                var dl_country = countryService.GetCountryBySpecificaiton(new CountrySpecification()
                {
                    CountryName = country
                }).FirstOrDefault();

                letter.ToAddress = new AddressInfo()
                {
                    Address1 = address,
                    Zipcode = postal,
                    City = city,
                    Country = dl_country,
                    State = state
                }; // TODO: Fix country
                letter.LetterContent = new LetterContent();
                if (usePdf)
                {
                    letter.LetterContent.Path = uploadFileKey;
                }
                else
                {
                    letter.LetterContent.Path = string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month,
                        Guid.NewGuid().ToString());
                    string filepath = PathHelper.GetAbsoluteFile(letter.LetterContent.Path);
                    content = HttpUtility.HtmlDecode(content);
                    content = HttpUtility.UrlDecode(content);
                    var convertedText = HelperMethods.Utf8FixString(content);
                    PdfHelper.ConvertToPdf(filepath, convertedText);
                }

                var price = priceService.GetPriceByLetter(letter);

                bool isValidCredits = false;
                decimal credits = 0;
                if (SessionHelper.Customer != null)
                {
                    credits = SessionHelper.Customer.CreditsLeft;
                    if (credits > 0)
                    {
                        isValidCredits = true;
                    }
                }

                return Json(new
                {
                    status = "success",
                    price = price,
                    numberOfPages = letter.LetterContent.PageCount,
                    credits = credits,
                    isAuthenticated = SessionHelper.Customer != null,
                    isValidCredits = isValidCredits
                });
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    status ="error",
                    message = "We cannot send this letter. Try a shorter letter.",
                    price = 0,
                    numberOfPages = 0,
                    credits = 0,
                    isAuthenticated = SessionHelper.Customer != null,
                    isOverCredits = false
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex);   
            }

            return Json(new
            {
                status = "error",
                price = 0,
                numberOfPages = 0,
                credits = 0,
                isAuthenticated = SessionHelper.Customer != null,
                isOverCredits = false
            });
        }

        [HttpPost]
        public JsonResult ApplyVoucher(string code)
        {
            try
            {
                decimal couponValueLeft = 0.0m;
                var coupon = couponService.GetCouponBySpecification(new CouponSpecification() {Code = code}).FirstOrDefault();
                if (coupon != null)
                {
                    couponValueLeft = coupon.CouponValueLeft;
                }
                return Json(new { couponValueLeft = couponValueLeft });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return Json(new { couponValueLeft = 0.0m });
        }

        public FileResult GeneratePDF(string content)
        {
            if (string.IsNullOrEmpty(content)) return File(new byte[0], "text/plain");

            content = HttpUtility.HtmlDecode(content);
            content = HttpUtility.UrlDecode(content);
            
            var convertedText = HelperMethods.Utf8FixString(content);
            var ms = PdfHelper.ConvertToPdf(convertedText);

            return File(ms, "application/pdf", "LetterAmazer_com.pdf");
        }

        public FileResult PreviewPDF(string key)
        {
            logger.DebugFormat("pdf key file: {0}", key);
            string filename = PathHelper.GetAbsoluteFile(key);
            return File(filename, "application/pdf", Path.GetFileName(filename));
        }

      
        public ActionResult Confirmation()
        {
            return View();
        }

        private string GetUploadFileName(string uploadFilename)
        {
            return string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
        }

    }
}
