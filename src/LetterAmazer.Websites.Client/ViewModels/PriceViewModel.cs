using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class PriceViewModel
    {
        public string SelectedItem { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public PriceViewModel()
        {
            this.Countries = new List<SelectListItem>();
        }
    }
}