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

namespace LetterAmazer.Websites.Client.Controllers
{
    public class SingleLetterController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SingleLetterController));

        private IOrderService orderService;
        public SingleLetterController(IOrderService orderService)
        {
            this.orderService = orderService;
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

                if (model.UseUploadFile)
                {
                    foreach (var file in model.UploadFiles)
                    {
                        Letter letter = new Letter();
                        letter.LetterStatus = LetterStatus.Created;
                        letter.LetterDetails.Add(letterDetail);
                        if (SecurityHelper.CurrentUser != null)
                        {
                            letter.CustomerId = SecurityHelper.CurrentUser.Id;
                            letter.Customer = SecurityHelper.CurrentUser;
                        }
                        letter.ToAddress = addressInfo;
                        letter.LetterContent.Path = Server.MapPath(string.Format("~/UserData/PdfLetters/{0}", file));

                        order.Letters.Add(letter);
                    }
                }
                else
                {
                    Letter letter = new Letter();
                    letter.LetterStatus = LetterStatus.Created;
                    letter.LetterDetails.Add(letterDetail);
                    if (SecurityHelper.CurrentUser != null)
                    {
                        letter.CustomerId = SecurityHelper.CurrentUser.Id;
                        letter.Customer = SecurityHelper.CurrentUser;
                    }
                    letter.ToAddress = addressInfo;
                    letter.LetterContent.WrittenContent = model.WriteContent;
                    string tempPath = Server.MapPath(string.Format("~/UserData/PdfLetters/{0}.pdf", Guid.NewGuid().ToString()));
                    PdfManager m = new PdfManager();
                    var convertedText = HelperMethods.Utf8FixString(model.WriteContent);
                    m.ConvertToPdf(tempPath, convertedText);
                    letter.LetterContent.Path = tempPath;

                    order.Letters.Add(letter);
                }

                OrderContext orderContext = new OrderContext();
                orderContext.Order = order;
                orderContext.SignUpNewsletter = model.SignUpNewsletter;
                string redirectUrl = orderService.CreateOrder(orderContext);

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
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
                string fileName = GetTempFileName(uploadFile.FileName);
                uploadFile.SaveAs(fileName);
                return Json(new
                {
                    status = "success",
                    key = Path.GetFileName(fileName)
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        private string GetTempFileName(string uploadFilename)
        {
            string filename = Path.GetFileName(uploadFilename);
            filename = GetRelativeTempFile(filename);
            int index = 1;
            while (System.IO.File.Exists(filename))
            {
                string ext = Path.GetExtension(uploadFilename);
                string name = Path.GetFileNameWithoutExtension(uploadFilename);
                filename = string.Format("{0}-{1}{2}", name, index, ext);
                filename = GetRelativeTempFile(filename);
                index++;
            }
            return filename;
        }

        private string GetRelativeTempFile(string filename)
        {
            return Server.MapPath("~/UserData/PdfLetters/" + filename);
        }
    }
}
