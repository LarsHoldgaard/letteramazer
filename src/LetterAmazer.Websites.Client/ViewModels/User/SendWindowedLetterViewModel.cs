﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class SendWindowedLetterViewModel
    {
        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }


        public bool IsLoggedIn { get; set; }
        public string Email { get; set; }
        public string SelectedOriginCountry { get; set; }

        public int DestinationCountryId { get; set; }

        public int? LetterSize { get; set; }
        public int? LetterType { get; set; }

        public bool UseUploadFile { get; set; }
        public string[] UploadFile { get; set; }
        public string WriteContent { get; set; }
        public decimal UserCredits { get; set; }

        public int PaymentMethodId { get; set; }

        public SendWindowedLetterViewModel()
        {
            this.Countries = new List<SelectListItem>();
            this.PaymentMethods = new List<SelectListItem>();
        }
    }
}