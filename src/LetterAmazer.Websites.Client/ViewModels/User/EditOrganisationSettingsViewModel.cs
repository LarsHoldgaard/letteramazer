﻿using System;
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
        public IEnumerable<SelectListItem> Countries { get; set; }

        public int LetterType { get; set; }
        public IEnumerable<SelectListItem> LetterTypes { get; set; }


    }
}