using System.Configuration;
using System.Linq;
using System.Net;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.PriceUpdater;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Thumbnail;
using LetterAmazer.Websites.Client.ViewModels.User;
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

        private IOfficeService officeService;
        private IOfficeProductService officeProductService;
        public SingleLetterController(IOrderService orderService, IPaymentService paymentService,
            ICouponService couponService, ICountryService countryService, IPriceService priceService,
            ICustomerService customerService,IPriceUpdater priceUpdater, IDeliveryJobService deliveryJobService,
            IOfficeService officeService, IOfficeProductService officeProductService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.couponService = couponService;
            this.countryService = countryService;
            this.priceService = priceService;
            this.customerService = customerService;
            this.priceUpdater = priceUpdater;
            this.deliveryJobService = deliveryJobService;
            
            this.officeProductService = officeProductService;
            this.officeService = officeService;
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
                
            CreateSingleLetterModel model = new CreateSingleLetterModel()
            {
                PaymentMethodId = 1,
                LetterType = (int)LetterType.Pres
            };

            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            foreach (var country in countries)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.Id.ToString(),
                };

                model.Countries.Add(selectedItem);
            }

            return View(model);
        }

        [HttpGet]
        public FileResult GetThumbnail(string uploadFileKey)
        {
            if (string.IsNullOrEmpty(uploadFileKey))
            {
                return new FileStreamResult(new MemoryStream(), "image/jpeg");
            }

            var basePath = Server.MapPath(ConfigurationManager.AppSettings["LetterAmazer.Settings.StoreThumbnail"]);
            uploadFileKey = PathHelper.GetAbsoluteFile(uploadFileKey);
            var thumbnailService = new ThumbnailGenerator(basePath);
            var byteData = System.IO.File.ReadAllBytes(uploadFileKey);
            var data = thumbnailService.GetThumbnailFromA4(byteData);

            return new FileStreamResult(new MemoryStream(data), "image/jpeg");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(CreateSingleLetterModel model)
        {
            try
            {
                ValidateInput();

                var order = CreateOrderFromViewModel(model);

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

        private Price addCouponlines(Price price, Coupon coupon, Order order)
        {
            return price;
            //Price rest = new Price();
            //rest.VatPercentage = price.VatPercentage;
            //rest.PriceExVat = price.PriceExVat;

            //if (coupon != null)
            //{
            //    decimal chargeCoupon = 0.0m;

            //    // if the price is higher than what is left on the coupon, the
            //    if (rest.Total > coupon.CouponValueLeft)
            //    {
            //        chargeCoupon = coupon.CouponValueLeft;
            //    }
            //    else
            //    {
            //        chargeCoupon = rest.Total;
            //    }

            //    order.OrderLines.Add(new OrderLine()
            //    {
            //        ProductType = ProductType.Payment,
            //        PaymentMethodId = 3, // coupon                        
            //        CouponId = coupon.Id,
            //        Price = new Price()
            //        {
            //            PriceExVat = chargeCoupon,
            //            VatPercentage = price.VatPercentage
            //        }
            //    });

            //    rest.Total -= coupon.CouponValueLeft;
            //}
            //return rest;
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
        public JsonResult GetPrice(bool usePdf, string uploadFileKey, string content, string address, string postal, string city, string state,int country)
        {
            try
            {
                Letter letter = new Letter();
                var dl_country = countryService.GetCountryById(country);

                letter.ToAddress = new AddressInfo()
                {
                    Address1 = address,
                    Zipcode = postal,
                    City = city,
                    Country = dl_country,
                    State = state
                };

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

                var priceSpec = new PriceSpecification()
                {
                    CountryId = letter.ToAddress.Country.Id,
                    LetterColor = LetterColor.Color,
                    LetterProcessing = LetterProcessing.Dull,
                    LetterPaperWeight = LetterPaperWeight.Eight,
                };

                if (SessionHelper.Customer != null)
                {
                    priceSpec.OfficeId =
                        SessionHelper.Customer.Organisation.RequiredOfficeId.HasValue
                            ? SessionHelper.Customer.Organisation.RequiredOfficeId.Value
                            : 0;
                }

                var selectedOfficeProductId = priceService.GetPriceBySpecification(priceSpec).OfficeProductId;
                
                var price = priceService.GetPriceBySpecification(new PriceSpecification()
                {
                    CountryId = letter.ToAddress.Country.Id,
                    PageCount = letter.LetterContent.PageCount,
                    OfficeProductId = selectedOfficeProductId
                });


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

        public Order CreateOrderFromViewModel(SendWindowedLetterViewModel model)
        {
            return CreateOrderFromViewModel(new CreateSingleLetterModel()
            {
                DestinationCountry = model.DestinationCountryId,
                UseUploadFile = model.UseUploadFile,
                SelectedCountry = model.SelectedCountry,
                WriteContent = model.WriteContent,
                Countries = model.Countries,
                UploadFile = model.UploadFile,
                PaymentMethodId = model.PaymentMethodId,
                LetterSize = model.LetterSize,
                LetterType = model.LetterType,
                Email = model.Email
            });
        }

        #region Windowed envelope



        [HttpPost]
        public ActionResult SendWindowedLetter(SendWindowedLetterViewModel model)
        {
            var order = new SingleLetterController(orderService, paymentService, couponService, countryService, priceService, customerService, null, null, officeService, officeProductService).
                CreateOrderFromViewModel(model);

            var updated_order = orderService.Create(order);

            string redirectUrl = paymentService.Process(updated_order);

            if (string.IsNullOrEmpty(redirectUrl))
            {
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                return RedirectToAction("Index", "User", dashboardViewModel);
            }

            return Redirect(redirectUrl);
        }

        #endregion


        public Order CreateOrderFromViewModel(CreateSingleLetterModel model)
        {
            if (string.IsNullOrEmpty(model.Email) && (SessionHelper.Customer == null || string.IsNullOrEmpty(SessionHelper.Customer.Email)))
            {
                throw new Exception("Cannot make an order without an e-mail");
            }

            AddressInfo addressInfo = new AddressInfo();
            addressInfo.Country = countryService.GetCountryById(int.Parse(model.SelectedCountry));
            addressInfo.Address1 = model.DestinationAddress;
            addressInfo.FirstName = model.RecipientName;
            addressInfo.State = model.DestinationState;
            addressInfo.City = model.DestinationCity;
            addressInfo.Zipcode = model.ZipCode;

            var priceSpec = new PriceSpecification()
            {
                CountryId = addressInfo.Country.Id,
                LetterColor = LetterColor.Color,
                LetterProcessing = LetterProcessing.Dull,
                LetterType = (LetterType) model.LetterType.Value,
                LetterPaperWeight = LetterPaperWeight.Eight,
            };
            if (SessionHelper.Customer != null)
            {
                priceSpec.OfficeId =
                    SessionHelper.Customer.Organisation.RequiredOfficeId.HasValue
                        ? SessionHelper.Customer.Organisation.RequiredOfficeId.Value
                        : 0;
            }

            var selectedOfficeProductId = priceService.GetPriceBySpecification(priceSpec).OfficeProductId;
            var selectedOfficeProduct = officeProductService.GetOfficeProductById(selectedOfficeProductId);

            LetterDetails letterDetail = new LetterDetails()
            {
                LetterColor = LetterColor.Color,
                LetterPaperWeight = LetterPaperWeight.Eight,
                LetterProcessing = LetterProcessing.Dull,
                LetterSize = selectedOfficeProduct.LetterDetails.LetterSize,
                LetterType = selectedOfficeProduct.LetterDetails.LetterType
            };

            Letter letter = new Letter()
            {
                LetterDetails = letterDetail,
                ToAddress = addressInfo,
            };

           
            Order order = new Order();

            if (SessionHelper.Customer != null)
            {
                            order.Customer = SessionHelper.Customer;
            }
            else
            {
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
            }

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
                // convert to lettersize if letter size
                if (selectedOfficeProduct.LetterDetails.LetterSize == LetterSize.Letter)
                {
                    PdfHelper.ConvertPdfSize(PathHelper.GetAbsoluteFile(letter.LetterContent.Path),LetterSize.A4, LetterSize.Letter);
                }
                letter.LetterContent.Content =
                    System.IO.File.ReadAllBytes(PathHelper.GetAbsoluteFile(letter.LetterContent.Path));
            }


            var price = priceService.GetPriceBySpecification(new PriceSpecification()
            {
                CountryId = addressInfo.Country.Id,
                PageCount = letter.LetterContent.PageCount,
                OfficeProductId = selectedOfficeProduct.Id
            });

            if (SessionHelper.Customer != null)
            {
                price.VatPercentage = SessionHelper.Customer.VatPercentage();
            }
            else
            {
                price.VatPercentage = 0.25m;
            }

            var officeProductId = price.OfficeProductId;
            letter.OfficeId = officeProductService.GetOfficeProductById(officeProductId).OfficeId;

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
                BaseProduct = letter,
                ProductType = ProductType.Letter,
                Price = new Price()
                {
                    PriceExVat = price.PriceExVat,
                    VatPercentage = price.VatPercentage
                }
            });



            var rest = addCouponlines(price, coupon, order);

            if (rest.Total > 0)
            {
                order.OrderLines.Add(new OrderLine()
                {
                    ProductType = ProductType.Payment,
                    PaymentMethodId =model.PaymentMethodId,
                    Price = new Price()
                    {
                        PriceExVat = rest.PriceExVat,
                        VatPercentage = price.VatPercentage
                    }
                });
            }

            return order;
        }

        private string GetUploadFileName(string uploadFilename)
        {
            return string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
        }

    }
}
