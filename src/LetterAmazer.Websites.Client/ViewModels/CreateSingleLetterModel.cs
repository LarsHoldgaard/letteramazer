using System.Web.Mvc;
using LetterAmazer.Websites.Client.Resources.Views.SingleLetter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class CreateSingleLetterModel
    {
        public CreateSingleLetterModel()
        {
            this.WriteContent = ViewRes.WriteYourLetterHere;
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

        public int LetterSize { get; set; }

        public string VoucherCode { get; set; }
        public bool SignUpNewsletter { get; set; }

        public bool UseUploadFile { get; set; }
        public string UploadFile { get; set; }
        public string WriteContent { get; set; }

        public bool HasCredits { get; set; }

        
    }
}