using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels.Home
{
    public class PriceDescriptionViewModel
    {
        public int CountryId { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string SeoTitle { get; set; }

        public PriceViewModel PriceViewModel { get; set; }

        public CountryPriceList CountryPriceList { get; set; }

        public PriceDescriptionViewModel()
        {
            this.PriceViewModel =new PriceViewModel();
            this.CountryPriceList = new CountryPriceList();
        }
    }
}