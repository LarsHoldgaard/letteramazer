using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels
{
    public class CreditsViewModel
    {
        public IEnumerable<PaymentMethodViewModel> PaymentMethods { get; set; }
        public int SelectedPaymentMethod { get; set; }

        public int PurchaseAmount { get; set; }

        public decimal Credit { get; set; }
        public decimal CreditLimit { get; set; }

        public CreditsViewModel()
        {
            this.PurchaseAmount = 50;
        }
    }

    public class PaymentMethodViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}