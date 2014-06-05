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
            this.PaymentMethods = new List<SelectListItem>();
            this.Countries = new List<SelectListItem>();
            this.To = DateTime.Now;
            this.From = DateTime.Now.AddDays(-1000);
        }

        public string[] SelectedInvoices { get; set; }

        public string SelectedCountry { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public List<SelectListItem> PaymentMethods { get; set; }
        public int PaymentMethodId { get; set; }

        public string AccountStatus { get; set; }
        public string Token { get; set; }

        public string AppUrl {get;set;}
        public int UserId { get; set; }
    }

    public class PartnerInvoiceViewModel
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string PdfUrl { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerCounty { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }

        
    }
}
