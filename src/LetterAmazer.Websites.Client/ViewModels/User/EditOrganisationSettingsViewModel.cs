using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class EditOrganisationSettingsViewModel
    {
        public int OrganisationId { get; set; }

        public string PreferedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public int LetterType { get; set; }
        public List<SelectListItem> LetterTypes { get; set; }

        public EditOrganisationSettingsViewModel()
        {
            this.Countries = new List<SelectListItem>();
            this.LetterTypes = new List<SelectListItem>();
        }
    }
}