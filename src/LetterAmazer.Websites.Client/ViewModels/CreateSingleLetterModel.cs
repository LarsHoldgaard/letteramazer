using System.Web.Mvc;
using System.Collections.Generic;
using LetterAmazer.Websites.Client.ViewModels.Home;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class CreateSingleLetterModel
    {
        public CountryPriceList CountryPriceList { get; set; }

        public CreateSingleLetterModel()
        {
            this.CountryPriceList = new CountryPriceList();
            this.WriteContent = "Write your letter here"; // TODO: why this here? should be in view
            this.Countries = new List<SelectListItem>();
        }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string RecipientName { get; set; }

        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }
        
        public int DestinationCountry { get; set; }
        public string DestinationState { get; set; }
        public string ZipCode { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationAddress { get; set; }
        public int PaymentMethodId { get; set; }

        public int? LetterSize { get; set; }
        public int? LetterType { get; set; }

        public bool SignUpNewsletter { get; set; }

        public bool UseUploadFile { get; set; }
        public string[] UploadFile { get; set; }
        public string WriteContent { get; set; }

        public bool HasCredits { get; set; }

        


    }
}