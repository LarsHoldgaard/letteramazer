using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Invoice
{
    public class InvoiceSpecification:Specifications
    {
        public int OrganisationId { get; set; }
        public int OrderId { get; set; }

        public InvoiceStatus? InvoiceStatus { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }
}
