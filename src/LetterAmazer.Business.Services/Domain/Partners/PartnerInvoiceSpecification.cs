using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public class PartnerInvoiceSpecification:Specifications
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
