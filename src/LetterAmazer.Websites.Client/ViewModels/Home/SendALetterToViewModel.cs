using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetterAmazer.Websites.Client.ViewModels.User;
using Microsoft.Ajax.Utilities;

namespace LetterAmazer.Websites.Client.ViewModels.Home
{
    public class SendALetterToViewModel
    {
        public int CountryId { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string SeoTitle { get; set; }

        public CountryPriceList CountryPriceList { get; set; }
        public SendWindowedLetterViewModel SendWindowedLetterViewModel { get; set; }

        public SendALetterToViewModel()
        {
            this.SendWindowedLetterViewModel = new SendWindowedLetterViewModel();
            this.CountryPriceList = new CountryPriceList();
        }
    }
}