using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Domain.Offices;
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

        public _Default()
        {
            this.fulfillmentPartnerService = new FulfillmentPartnerService(new LetterAmazerEntities(), new FulfilmentPartnerFactory());
            this.officeService = new OfficeService(new LetterAmazerEntities(),
                new OfficeFactory(new CountryService(new LetterAmazerEntities(), new CountryFactory())));
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
}