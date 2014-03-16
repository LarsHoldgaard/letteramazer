using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using LetterAmazer.Business.Services.Domain.Invoice;

namespace LetterAmazer.Websites.Client.ViewModels.User
{
    public class InvoiceOverviewViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }



        public List<InvoiceSnippetViewModel> InvoiceSnippets { get; set; }

        public InvoiceOverviewViewModel()
        {
            this.InvoiceSnippets = new List<InvoiceSnippetViewModel>();
        }
    }


    public class InvoiceSnippetViewModel
    {
        public Guid InvoiceGuid { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TotalPrice { get; set; }

        public InvoiceStatus Status { get; set; }
    }

}