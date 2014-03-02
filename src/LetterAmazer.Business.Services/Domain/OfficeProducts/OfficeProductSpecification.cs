using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;
using LetterAmazer.Business.Services.Domain.Pricing;

namespace LetterAmazer.Business.Services.Domain.OfficeProducts
{
    public class OfficeProductSpecification:Specifications
    {
        public int Id { get; set; }

        public ProductMatrixReferenceType? ProductMatrixReferenceType { get; set; }

    }
}
