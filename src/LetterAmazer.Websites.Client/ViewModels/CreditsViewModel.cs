using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class CreditsViewModel
    {
        public decimal Credits { get; set; }
        public decimal CreditLimit { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Funds { get; set; }
    }
}