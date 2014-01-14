using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetterAmazer.Websites.Client.ViewModels;
using LetterAmazer.Websites.Client.Helpers;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Services.LetterContent;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Business.Services.Model;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Services.PaymentMethod;
using System.Text;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class SingleLetterController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SingleLetterController));

        private IOrderService orderService;
        private IPaymentService paymentService;
        private ILetterService letterService;
        private ICouponService couponService;
        public SingleLetterController(IOrderService orderService, IPaymentService paymentService, 
            ILetterService letterService, ICouponService couponService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.letterService = letterService;
            this.couponService = couponService;
        }

        [HttpGet]
        public ActionResult Index()
        {
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
                order.Email = model.Email;
                order.Phone = model.Phone;
                order.PaymentMethod = PaypalMethod.NAME;
                order.CouponCode = model.VoucherCode;

                AddressInfo addressInfo = new AddressInfo();
                addressInfo.Address = model.DestinationAddress;
                addressInfo.FirstName = model.RecipientName;
                addressInfo.City = model.DestinationCity;
                addressInfo.Country = model.DestinationCountry;
                addressInfo.CountryCode = model.DestinationCountryCode;
                addressInfo.Postal = model.ZipCode;

                LetterDetail letterDetail = new LetterDetail();
                letterDetail.Color = Color.Color;
                letterDetail.LetterQuality = LetterQuatity.Normal;
                letterDetail.PaperQuality = PaperQuality.Normal;
                letterDetail.PrintQuality = PrintQuality.Normal;
                letterDetail.Size = PrintSize.A4;

                OrderItem orderItem = new OrderItem();

                Letter letter = new Letter();
                letter.LetterStatus = LetterStatus.Created;
                letter.LetterDetail = letterDetail;
                if (SecurityHelper.CurrentUser != null)
                {
                    letter.CustomerId = SecurityHelper.CurrentUser.Id;
                    letter.Customer = SecurityHelper.CurrentUser;
                }
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
                string filename = GetAbsoluteFile(keyName);
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
                PdfManager pdfManager = new PdfManager();

                Letter letter = new Letter();
                letter.ToAddress = new AddressInfo() { Address = address, Postal = postal, City = city, Country = country };
                letter.LetterContent = new LetterContent();
                if (usePdf)
                {
                    letter.LetterContent.Path = uploadFileKey;
                }
                else
                {
                    letter.LetterContent.Path = string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month, Guid.NewGuid().ToString());
                    string filepath = GetAbsoluteFile(letter.LetterContent.Path);
                    content = HttpUtility.HtmlDecode(content);
                    content = HttpUtility.UrlDecode(content);
                    var convertedText = HelperMethods.Utf8FixString(content);
                    pdfManager.ConvertToPdf(filepath, convertedText);
                }
                var pages = pdfManager.GetPagesCount(GetAbsoluteFile(letter.LetterContent.Path));
                var price = letterService.GetCost(letter);
                return Json(new { 
                    price = price ,
                    numberOfPages = pages 
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex);   
            }

            return Json(new
            {
                price = 0,
                numberOfPages = 0
            });
        }

        [HttpPost]
        public JsonResult ApplyVoucher(string code)
        {
            try
            {
                decimal couponValueLeft = 0.0m;
                var coupon = couponService.GetCoupon(code);
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
            PdfManager m = new PdfManager();
            var convertedText = HelperMethods.Utf8FixString(content);
            var ms = m.ConvertToPdf(convertedText);

            return File(ms, "application/pdf", "LetterAmazer_com.pdf");
        }

        public FileResult PreviewPDF(string key)
        {
            logger.DebugFormat("pdf key file: {0}", key);
            string filename = GetAbsoluteFile(key);
            return File(filename, "application/pdf", Path.GetFileName(filename));
        }

        public JsonResult PaypalIpn()
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
                    catch (Exception)
                    {
                    }
                }
                if (readSuccess == false) return Json(new { status = "error", message = "Can not read data from paypal" }, JsonRequestBehavior.AllowGet);
                
                //TODO We should mark the order should call to paypal again!
                string strRequest = Encoding.ASCII.GetString(param);
                VerifyPaymentResult result = paymentService.Get(PaypalMethod.NAME).Verify(new VerifyPaymentContext() { Parameters = strRequest });

                orderService.MarkOrderIsPaid(result.OrderId);

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

        private string GetAbsoluteFile(string filename)
        {
            return Server.MapPath(letterService.GetRelativeLetterStoragePath().TrimEnd('/') + "/" + filename);
        }
    }
}
