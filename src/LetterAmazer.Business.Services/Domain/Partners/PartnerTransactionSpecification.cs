using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public class PartnerTransactionSpecification:Specifications
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int PartnerId { get; set; }

        public int ValueId { get; set; }
    }
}
