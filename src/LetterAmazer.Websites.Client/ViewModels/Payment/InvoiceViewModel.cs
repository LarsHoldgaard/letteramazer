using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.ViewModels.Payment
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public InvoiceAddressViewModel CompanyInfo { get; set; }
        public InvoiceAddressViewModel ReceiverInfo { get; set; }

        public DateTime DateCreated { get; set; }

        public List<InvoiceLineViewModel> Lines { get; set; }

        public decimal Total { get; set; }
        public decimal VatTotal { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal TotalExVat { get; set; }

        public InvoiceViewModel()
        {
            this.Lines = new List<InvoiceLineViewModel>();
        }
    }

    public class InvoiceAddressViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string VatNumber { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }

    public class InvoiceLineViewModel
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}