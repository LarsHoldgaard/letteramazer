using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels.Payment
{
    public class EpayViewModel
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string GoogleTracker { get; set; }
        public string DeclineUrl { get; set; }
        public string AcceptUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string MerchantNumber { get; set; }

        public string AmountInMinorUnits
        {
            get
            {
                var amountTotal = Amount*100;
                amountTotal = Math.Round(amountTotal, 0);
                return amountTotal.ToString();
            }
        }
    }
}