using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.ViewModels.Partner
{
    public class PartnerInvoiceOverviewViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<PartnerInvoiceViewModel> PartnerInvoices { get; set; }

        public PartnerInvoiceOverviewViewModel()
        {
            this.PartnerInvoices = new List<PartnerInvoiceViewModel>();
            this.To = DateTime.Now;
            this.From = DateTime.Now.AddDays(-1000);
        }

        public string[] SelectedInvoices { get; set; }
    }

    public class PartnerInvoiceViewModel
    {
        
        public string PdfUrl { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
