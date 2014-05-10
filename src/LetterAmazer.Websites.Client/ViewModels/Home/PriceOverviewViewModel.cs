using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels.Home
{
    public class PriceOverviewViewModel
    {
        public CountryPriceList CountryPriceList { get; set; }
        public PriceViewModel PriceViewModel { get; set; }

        public PriceOverviewViewModel()
        {
            this.CountryPriceList = new CountryPriceList();
            this.PriceViewModel = new PriceViewModel();
        }
    }

    public class CountryPriceList
    {
        public List<CountryPriceViewModel> CountryPriceViewModel { get; set; }

        public CountryPriceList()
        {
            this.CountryPriceViewModel = new List<CountryPriceViewModel>();
        }
    }

    public class CountryPriceViewModel
    {
        public string Name { get; set; }
        public string Alias { get; set; }
    }
}