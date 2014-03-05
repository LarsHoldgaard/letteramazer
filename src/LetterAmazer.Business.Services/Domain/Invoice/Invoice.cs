using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.AddressInfos;

namespace LetterAmazer.Business.Services.Domain.Invoice
{
    public class Invoice
    {
        /// <summary>
        /// This is the address info of the person receiving the invoice
        /// </summary>
        public AddressInfo ReceiverInfo { get; set; }

        /// <summary>
        /// This is the address info of the companys ending the invoice (LetterAmazer)
        /// </summary>
        public AddressInfo InvoiceInfo { get; set; }

        public int Id { get; set; }
        public Guid Guid { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateDue { get; set; }

        public InvoiceStatus InvoiceStatus { get; set; }

        public string InvoiceNumber { get; set; }

        public int OrderId { get; set; }
        public int OrganisationId { get; set; }

        public List<InvoiceLine> InvoiceLines { get; set; }

        public decimal PriceExVat { get; set; }
        public decimal PriceVatPercentage { get; set; }
        public decimal PriceTotal { get; set; }

        public decimal PriceVat { get; set; }

        public Invoice()
        {
            this.InvoiceLines = new List<InvoiceLine>();
            this.InvoiceStatus = InvoiceStatus.Created;
        }
    }
}
