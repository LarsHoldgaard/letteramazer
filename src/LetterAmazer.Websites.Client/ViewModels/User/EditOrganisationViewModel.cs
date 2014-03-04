using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class EditOrganisationViewModel
    {
        public string OrganisationName { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string VatNumber { get; set; }

        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public EditOrganisationViewModel()
        {
            this.Countries = new List<SelectListItem>();
        }
    }
}