using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public AddressInfoDTO ReceiverInfo { get; set; }

        /// <summary>
        /// This is the address info of the companys ending the invoice (LetterAmazer)
        /// </summary>
        public AddressInfoDTO InvoiceInfo { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDue { get; set; }

        public string InvoiceStatus { get; set; }

        public string InvoiceNumber { get; set; }

        public int OrderId { get; set; }
        public int OrganisationId { get; set; }

        public List<InvoiceLineDTO> InvoiceLines { get; set; }

        public decimal PriceExVat { get; set; }
        public decimal PriceVatPercentage { get; set; }
        public decimal PriceTotal { get; set; }

        public decimal PriceVat { get; set; }

        public string InvoicePaymentMessage { get; set; }
    }
}
