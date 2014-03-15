using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class CreditsViewModel
    {
        public List<SelectListItem> PaymentMethods { get; set; }
        public string SelectedPaymentMethod { get; set; }

        public int PurchaseAmount { get; set; }

        public decimal Credit { get; set; }
        public decimal CreditLimit { get; set; }

        public CreditsViewModel()
        {
            this.PaymentMethods = new List<SelectListItem>();
            this.PurchaseAmount = 50;
        }
    }
}