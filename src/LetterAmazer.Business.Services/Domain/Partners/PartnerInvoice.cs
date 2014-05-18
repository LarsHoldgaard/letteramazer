using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Pricing;

namespace LetterAmazer.Business.Services.Domain.Partners
{

    public class PartnerInvoice
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string PdfUrl { get; set; }
        public DateTime InvoiceDate { get; set; }

        public Price Price { get; set; }
        public string CustomerName { get; set; }
        public string LetterAmazerStatus { get; set; }
    }
}
