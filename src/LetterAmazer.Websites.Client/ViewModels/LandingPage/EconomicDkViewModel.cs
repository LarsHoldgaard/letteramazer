using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels.LandingPage
{
    public class EconomicDkViewModel
    {
        [EmailAddress(ErrorMessage = "This is not a valid e-mail address")]
        [Required(ErrorMessage = "You need to enter a valid e-mail address")]
        public string Email { get; set; }

        public string Password { get; set; }


        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public EconomicDkViewModel()
        {
            this.Countries = new List<SelectListItem>();
        }

    }
}