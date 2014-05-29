using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using iTextSharp.text;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Checkout;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.Envelope;
using LetterAmazer.Business.Services.Domain.Files;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.PriceUpdater;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Domain.Session;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Thumbnail;
using LetterAmazer.Websites.Client.Helpers;
using LetterAmazer.Websites.Client.ViewModels.Home;
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
        private ICountryService countryService;
        private IPriceService priceService;
        private ICustomerService customerService;
        private ICheckoutService checkoutService;
        private ISessionService sessionService;

        private IOfficeService officeService;
        private IOfficeProductService officeProductService;
        private IFileService fileService;
        private IEnvelopeService envelopeService;

        public SingleLetterController(IOrderService orderService, IPaymentService paymentService,
            ICountryService countryService, IPriceService priceService,ICustomerService customerService, IOfficeService officeService, 
            IOfficeProductService officeProductService, ICheckoutService checkoutService,ISessionService sessionService, IFileService fileService,IEnvelopeService envelopeService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.countryService = countryService;
            this.priceService = priceService;
            this.customerService = customerService;
            this.officeProductService = officeProductService;
            this.officeService = officeService;
            this.checkoutService = checkoutService;
            this.sessionService = sessionService;
            this.fileService = fileService;
            this.envelopeService = envelopeService;
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

            Helper.FillCountries(countryService,model.Countries);

            // TODO: maybe cleanup?
            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });
            foreach (var country in countries)
            {
                model.CountryPriceList.CountryPriceViewModel.Add(new CountryPriceViewModel()
                {
                    Alias = country.Alias,
                    Name = country.Name
                });
            }

            return View(model);
        }

        [HttpGet]
        public FileResult GetThumbnail(string[] uploadFileKey)
        {
            var stringPath = uploadFileKey[0];

            // if more than one image, don't show it (for now)
            if (stringPath.Contains(","))
            {
                return new FileStreamResult(new MemoryStream(), "image/jpeg");
            }

            if (string.IsNullOrEmpty(stringPath))
            {
                return new FileStreamResult(new MemoryStream(), "image/jpeg");
            }

            var data = fileService.GetFileById(stringPath,FileUploadMode.Temporarily);

            var envelope = envelopeService.GetEnvelopeById(1);
            var envelopeWindow = envelope.EnvelopeWindows[LetterSize.A4];
            var basePath = Server.MapPath(ConfigurationManager.AppSettings["LetterAmazer.Settings.StoreThumbnail"]);
            var thumbnailService = new ThumbnailGenerator(basePath);


            var imageData = thumbnailService.GetThumbnailFromA4(data, (int)envelopeWindow.WindowXOffset, (int)envelopeWindow.WindowYOffset,
                (int)envelopeWindow.WindowLength, (int)envelopeWindow.WindowHeight);


            return new FileStreamResult(new MemoryStream(imageData), "image/jpeg");
        }

        [HttpPost, ValidateInput(false)]
        [Obsolete]
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

        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                HttpPostedFileBase uploadFile = Request.Files[0];

                var fileStorageName = uploadFile.FileName;
                var keyName = fileService.Create(Business.Services.Utils.Helpers.GetBytes(uploadFile.InputStream),
                    fileStorageName,FileUploadMode.Temporarily);
                
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
        public JsonResult GetPriceFromUrl(string pdfUrl,
            string country)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var customerId = SessionHelper.Customer != null ? SessionHelper.Customer.Id : 0;
                    var data = client.DownloadData(pdfUrl);

                    var fileKey = fileService.Create(data, Business.Services.Utils.Helpers.GetUploadDateString(Guid.NewGuid().ToString()));

                    var price = priceService.GetPricesFromFiles(new[] { fileKey }, customerId, int.Parse(country));

                    return Json(new
                    {
                        status = "success",
                        price = price,
                        isAuthenticated = SessionHelper.Customer != null
                    });
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = "error",
                    message = "We cannot send this letter. Right now we only support Denmark and 1-7 pages pr letter",
                    price = 0,
                    numberOfPages = 0,
                    isAuthenticated = SessionHelper.Customer != null,
                });
            }
        }

        [HttpPost]
        public JsonResult GetPrice(string uploadFileKey,
            int country)
        {
            try
            {
                //// TODO: stop being a fuck-tard and dont call this json removal method
                string[] uploadFileKey2 = HelperMethods.RemoveJsonFromEntries(uploadFileKey);
                var customerId = SessionHelper.Customer != null ? SessionHelper.Customer.Id : 0;
                Price price = priceService.GetPricesFromFiles(uploadFileKey2, customerId, country);
                
                return Json(new
                {
                    status = "success",
                    price = price,
                    isAuthenticated = SessionHelper.Customer != null
                });
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    status = "error",
                    message = "We cannot send this letter. Right now we only support Denmark and 1-7 pages pr letter",
                    price = 0,
                    numberOfPages = 0,
                    isAuthenticated = SessionHelper.Customer != null,
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
                isAuthenticated = SessionHelper.Customer != null,
            });
        }

    

     
        public FileResult GeneratePDF(string content)
        {
            if (string.IsNullOrEmpty(content)) return File(new byte[0], "text/plain");


            // TODO: move to service layer
            content = HttpUtility.HtmlDecode(content);
            content = HttpUtility.UrlDecode(content);

            var convertedText = HelperMethods.Utf8FixString(content);
            var ms = PdfHelper.ConvertToPdf(convertedText);

            return File(ms, "application/pdf", "LetterAmazer_com.pdf");
        }

        public FileResult PreviewPDF(string key)
        {
            throw new NotImplementedException();
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
            //// TODO: stop being a fuck-tard
            model.UploadFile = model.UploadFile[0].Split(';');

            var order = new SingleLetterController(orderService, paymentService, countryService, priceService, customerService, officeService, officeProductService, checkoutService, sessionService, fileService, envelopeService).
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
            if (string.IsNullOrEmpty(model.Email) &&
                (SessionHelper.Customer == null || string.IsNullOrEmpty(SessionHelper.Customer.Email)))
            {
                throw new Exception("Cannot make an order without an e-mail");
            }

            Checkout checkout = new Checkout()
            {
                UserId = SessionHelper.Customer != null ? SessionHelper.Customer.Id : 0,
                Email = model.Email,
                PaymentMethodId = model.PaymentMethodId
            };
            
            foreach (var uploadFile in model.UploadFile)
            {
                var customerId = SessionHelper.Customer != null ? SessionHelper.Customer.Id : 0;
                var priceInfo = priceService.GetPricesFromFiles(new[] { uploadFile }, customerId, model.DestinationCountry);
                var fileBytes = fileService.GetFileById(uploadFile, FileUploadMode.Temporarily);
                var officeProduct = officeProductService.GetOfficeProductById(priceInfo.OfficeProductId);

                var t = new CheckoutLine()
                {
                    OfficeProductId = priceInfo.OfficeProductId,
                    Letter = new Letter()
                    {
                        ToAddress = new AddressInfo()
                        {
                            Country = countryService.GetCountryById(model.DestinationCountry)
                        },
                        LetterContent = new LetterContent()
                        {
                            Path = uploadFile,
                            Content = fileBytes
                        },
                        OfficeId = officeProduct.OfficeId
                    }
                };
                checkout.CheckoutLines.Add(t);                
            }

            return checkoutService.ConvertCheckout(checkout);
        }


    }
}
