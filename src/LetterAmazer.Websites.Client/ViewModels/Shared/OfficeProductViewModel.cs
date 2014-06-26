using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels.Shared
{
    public class OfficeProductViewModel
    {
        public int Id { get; set; }
        public PriceViewModel MyProperty { get; set; }

        public bool Enabled { get; set; }
        public EnvelopeViewModel Envelope { get; set; }
    }

    public class OfficeProductPriceViewModel {
        public decimal PriceExVat { get; set; }
        public decimal VatPercentage { get; set; }
        public string Currency { get; set; }
        public decimal Total { get; set; }
    }

    public class EnvelopeViewModel {
        public int Id { get; set; }
        public string Quality { get; set; }
        public string PaperSize { get;set;}
        public string Type { get; set; }
    }
}