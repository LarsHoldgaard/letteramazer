using System.Web.Http;
using System.Web.Mvc;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Websites.Client.ViewModels.Api;

namespace LetterAmazer.Websites.Client.Controllers.Api
{
    public class OfficeProductController : ApiController
    {
        private IOfficeProductService officeProductService;
        public OfficeProductController(IOfficeProductService officeProductService)
        {
            this.officeProductService = officeProductService;
        }

        [System.Web.Http.HttpPost]
        public ActionResult Create(OfficeProductViewModel model)
        {
            var officeProduct = new OfficeProduct()
            {
                CountryId = 1,
                ProductScope = ProductScope.Single,
                LetterDetails = new LetterDetails()
                {
                    LetterColor = LetterColor.BlackWhite,
                    LetterPaperWeight = LetterPaperWeight.Eight,
                    LetterProcessing = LetterProcessing.Dull,
                    LetterSize = LetterSize.A4,
                    LetterType = LetterType.Pres
                },
                OfficeId = 1
            };

            officeProductService.Create(officeProduct);

            return new JsonResult();
        }

        [System.Web.Http.HttpPost]
        public ActionResult Test(string id)
        {
            return new ContentResult() { Content = id };
        }

        [System.Web.Http.HttpGet]
        public ActionResult Gettest()
        {
            return new ContentResult() {Content ="hej"};
        }
    }
}
