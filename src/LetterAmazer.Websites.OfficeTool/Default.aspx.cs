using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Websites.OfficeTool
{
    public partial class _Default : Page
    {
        private IFulfillmentPartnerService fulfillmentPartnerService;
        private IOfficeService officeService;
        private IProductMatrixService productMatrixService;
        private IOfficeProductService offerProductService;
        private ICountryService countryService;

        public _Default()
        {
            this.fulfillmentPartnerService = new FulfillmentPartnerService(new LetterAmazerEntities(), new FulfilmentPartnerFactory());
            this.officeService = new OfficeService(new LetterAmazerEntities(),
                new OfficeFactory(new CountryService(new LetterAmazerEntities(), new CountryFactory())));
            this.productMatrixService = new ProductMatrixService(new LetterAmazerEntities(), new ProductMatrixFactory());
            this.offerProductService = new OfficeProductService(new LetterAmazerEntities(), new OfficeProductFactory(new ProductMatrixService(new LetterAmazerEntities(), new ProductMatrixFactory())));
            this.countryService = new CountryService(new LetterAmazerEntities(), new CountryFactory());
        }

        public List<FulfilmentPartner> FulfilmentPartners
        {
            get
            {
                var partners =
                    fulfillmentPartnerService.GetFulfillmentPartnersBySpecifications(
                        new FulfillmentPartnerSpecification() { Take = 10000 });
                return partners;
            }
        }

        public List<Office> Offices
        {
            get
            {
                if (string.IsNullOrEmpty(FulfilmentPartnersDdl.SelectedValue))
                {
                    return new List<Office>();
                }
                var partnerId = int.Parse(FulfilmentPartnersDdl.SelectedValue);

                var offices = officeService.GetOfficeBySpecification(new OfficeSpecification()
                {
                    FulfilmentPartnerId = partnerId
                });


                return offices;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CountryDll.DataSource = countryService.GetCountryBySpecificaiton(new CountrySpecification() { Take = 250 });
                CountryDll.DataTextField = "Name";
                CountryDll.DataValueField = "Id";
                CountryDll.DataBind();

                ContinentsDdl.DataSource = countryService.GetContinents();
                ContinentsDdl.DataTextField = "Name";
                ContinentsDdl.DataValueField = "Id";
                ContinentsDdl.DataBind();

                FulfilmentPartnersDdl.DataSource = FulfilmentPartners;
                FulfilmentPartnersDdl.DataTextField = "Name";
                FulfilmentPartnersDdl.DataValueField = "Id";
                FulfilmentPartnersDdl.DataBind();

                OfficesDdl.DataSource = Offices;
                OfficesDdl.DataTextField = "Title";
                OfficesDdl.DataValueField = "Id";
                OfficesDdl.DataBind();

                LetterColorDdl.DataSource = Enum.GetNames(typeof(LetterColor));
                LetterColorDdl.DataBind();

                LetterTypeDdl.DataSource = Enum.GetNames(typeof(LetterType));
                LetterTypeDdl.DataBind();

                LetterProcessingDdl.DataSource = Enum.GetNames(typeof(LetterProcessing));
                LetterProcessingDdl.DataBind();

                LetterWeightDdl.DataSource = Enum.GetNames(typeof(LetterPaperWeight));
                LetterWeightDdl.DataBind();

                LetterSizeDdl.DataSource = Enum.GetNames(typeof(LetterSize));
                LetterSizeDdl.DataBind();
            }
            

        }

        private Dictionary<string, decimal> GetEnteredOrderlines()
        {
            Dictionary<string, decimal> data = new Dictionary<string, decimal>();
           
            int i = 0;
            while (true)
            {
                var title = string.Format("orderlines_title_{0}", i);
                var value = string.Format("orderlines_value_{0}", i);

                var titleCtl = Request[title];
                var valueCtl = Request[value];

                if (titleCtl == null || valueCtl == null || string.IsNullOrEmpty(titleCtl))
                {
                    break;
                }

                valueCtl = valueCtl.Replace(".", ",");

                decimal valueOrder = 0.0m;
                decimal.TryParse(valueCtl, out valueOrder);

                if (valueOrder > 0)
                {
                    data.Add(titleCtl, valueOrder);
                }

                
                i++;
            }

            return data;
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            var letterDetails = new LetterDetails()
            {
                LetterColor = (LetterColor)(Enum.Parse(typeof(LetterColor), LetterColorDdl.SelectedValue)),
                LetterPaperWeight = (LetterPaperWeight)(Enum.Parse(typeof(LetterPaperWeight), LetterWeightDdl.SelectedValue)),
                LetterProcessing = (LetterProcessing)(Enum.Parse(typeof(LetterProcessing), LetterProcessingDdl.SelectedValue)),
                LetterSize = (LetterSize)(Enum.Parse(typeof(LetterSize), LetterSizeDdl.SelectedValue)),
                LetterType = (LetterType)(Enum.Parse(typeof(LetterType), LetterTypeDdl.SelectedValue))
            
            };

            var officeProd = new OfficeProduct()
            {
                OfficeId = int.Parse(OfficesDdl.SelectedValue),
                LetterDetails = letterDetails,
                
            };
            if (TypeOfLocationRbl.SelectedValue == "Continent")
            {
                officeProd.ProductScope = ProductScope.Continent;
                officeProd.ContinentId = int.Parse(ContinentsDdl.SelectedValue);
            }
            else if (TypeOfLocationRbl.SelectedValue == "Country")
            {
                officeProd.ProductScope = ProductScope.Country;
                officeProd.CountryId = int.Parse(CountryDll.SelectedValue);
            }
            else
            {
                officeProd.ProductScope = ProductScope.RestOfWorld;
            }

            var prod = offerProductService.Create(officeProd);

            var matrix = new ProductMatrix()
            {
                PriceType = ProductMatrixPriceType.FirstPage,
                ReferenceType = ProductMatrixReferenceType.Contractor,
                ValueId = prod.Id,
                ProductLines = new List<ProductMatrixLine>()
            };

            var orderLines = GetEnteredOrderlines();
            foreach (var orderLine in orderLines)
            {
                matrix.ProductLines.Add(new ProductMatrixLine()
                {
                    BaseCost = orderLine.Value,
                    LineType =  ProductMatrixLineType.Service,
                    Title = orderLine.Key
                });
            }

            productMatrixService.Create(matrix);
        }
    }
}