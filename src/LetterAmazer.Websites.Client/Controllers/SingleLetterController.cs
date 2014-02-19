using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.PriceUpdater;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Services;
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
        private IOrderLineService orderLineService;

        private IPaymentService paymentService;
        private ILetterService letterService;
        private ICouponService couponService;
        private ICustomerService customerService;
        private ICountryService countryService;
        private IPriceService priceService;
        private IPriceUpdater priceUpdater;

        public SingleLetterController(IOrderService orderService, IPaymentService paymentService,
            ILetterService letterService, ICouponService couponService, ICustomerService customerService,
            ICountryService countryService, IPriceService priceService, IOrderLineService orderLineService, IPriceUpdater priceUpdater)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.letterService = letterService;
            this.couponService = couponService;
            this.customerService = customerService;
            this.countryService = countryService;
            this.priceService = priceService;
            this.orderLineService = orderLineService;
            this.priceUpdater = priceUpdater;
        }

        [HttpGet]
        public ActionResult Index()
        {
          
            if (SessionHelper.Customer != null) return RedirectToAction("SendALetter", "User");
            //priceUpdater.Execute();
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
                addressInfo.PostalCode = model.ZipCode;

                var price = priceService.GetPriceByAddress(addressInfo);

                Customer customer = new Customer();
                customer.Email = model.Email;
                customer.Phone = model.Phone;
                customer.CustomerInfo = addressInfo;



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
                    OfficeProductId = price.OfficeProductId
                };

                order.Customer = customer;


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

                var stored_order = orderService.Create(order);

                var orderLine = new OrderLine()
                {
                    Quantity = 1,
                    ProductType = ProductType.Order,
                    BaseProduct = letter,
                    OrderId = stored_order.Id,
                };

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


                orderLineService.Create(orderLine);

                var paymentMethods = new List<PaymentMethods>();

                if (coupon != null)
                {
                    paymentMethods.Add(PaymentMethods.Coupon);
                }

                paymentMethods.Add(PaymentMethods.PayPal);

                string redirectUrl = paymentService.Process(paymentMethods, order);
                logger.Debug("redirectUrl: " + redirectUrl);
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
        public JsonResult GetPrice(bool usePdf, string uploadFileKey, string content, string address, string postal, string city, string country)
        {
            try
            {
                Letter letter = new Letter();
                var dl_country = countryService.GetCountryBySpecificaiton(new CountrySpecification()
                {
                    CountryName = country
                }).FirstOrDefault();

                letter.ToAddress = new AddressInfo() { Address1 = address, PostalCode = postal, City = city, Country = dl_country }; // TODO: Fix country
                letter.LetterContent = new LetterContent();
                if (usePdf)
                {
                    letter.LetterContent.Path = uploadFileKey;
                }
                else
                {
                    letter.LetterContent.Path = string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
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
                }

                return Json(new {
                    status = "success",
                    price = price,
                    numberOfPages = letter.LetterContent.PageCount,
                    credits = credits,
                    isAuthenticated = !(SessionHelper.Customer == null),
                    isValidCredits = isValidCredits
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

        public JsonResult PaypalIpn(string id)
        {
            try
            {
                logger.Info("IPN called");
                byte[] param = null; // try three time
                bool readSuccess = false;
                for (int i = 0; i < 3; i++)
                {
                    if (readSuccess == true) break;
                    try
                    {
                        param = Request.BinaryRead(Request.ContentLength);
                        readSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        logger.DebugFormat("try {0}", i);
                    }
                }
                if (readSuccess == false)
                {
                    logger.Debug("Can not read data from paypal");
                    return Json(new { status = "error", message = "Can not read data from paypal" }, JsonRequestBehavior.AllowGet);
                }

                ////TODO We should mark the order should call to paypal again!
                //string strRequest = Encoding.ASCII.GetString(param);
                //VerifyPaymentResult result = paymentService.Get(PaypalMethod.NAME).Verify(new VerifyPaymentContext() { Parameters = strRequest });

                //orderService.MarkOrderIsPaid(result.OrderId);
                //if (id.ToLower() == OrderType.AddFunds.ToString().ToLower())
                //{
                //    orderService.AddFundsForAccount(result.OrderId);
                //}

                return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        private string GetUploadFileName(string uploadFilename)
        {
            return string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
            //string filename = Path.GetFileName(uploadFilename);
            //filename = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, filename);
            //string keyName = filename;
            //filename = GetAbsoluteFile(filename);
            //int index = 1;
            //while (System.IO.File.Exists(filename))
            //{
            //    string ext = Path.GetExtension(uploadFilename);
            //    string name = Path.GetFileNameWithoutExtension(uploadFilename);
            //    filename = string.Format("{0}-{1}{2}", name, index, ext);
            //    filename = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, filename);
            //    keyName = filename;
            //    filename = GetAbsoluteFile(filename);
            //    index++;
            //}
            //return keyName;
        }

    }
}
