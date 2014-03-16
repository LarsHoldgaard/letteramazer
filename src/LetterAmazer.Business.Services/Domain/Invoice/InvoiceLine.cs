using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Invoice
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int InvoiceId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceExVat { get; set; }
    }
}
