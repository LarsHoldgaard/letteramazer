using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class PriceViewModel
    {
        public string SelectedLetterSize { get; set; }
        public List<SelectListItem> SelectedLetterSizes { get; set; }


        public string SelectedItem { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public PriceViewModel()
        {
            this.SelectedLetterSizes = new List<SelectListItem>();
            this.Countries = new List<SelectListItem>();
        }
    }
}