using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Business.Services.Services.Caching;
using LetterAmazer.Data.Repository.Data;
using LetterAmazer.Websites.Client.Helpers;

namespace LetterAmazer.Websites.Client.ViewModels.Shared
{
    public class TryServiceViewModel
    {
        [Required(ErrorMessage = "You need to enter a company name")]
        public string CompanyName { get; set; }

        [EmailAddress(ErrorMessage = "This is not a valid e-mail address")]
        [Required(ErrorMessage = "You need to enter a valid e-mail address")]
        public string Email { get; set; }

        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public string LettersAWeek { get; set; }

        public string PhoneNumber { get; set; }

        public TryServiceViewModel()
        {
            this.Countries = new List<SelectListItem>();
            Helper.FillCountries(new CountryService(new LetterAmazerEntities(), new CountryFactory(), new HttpCacheService()), Countries,59);
        }
    }
}