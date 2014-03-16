using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Services.Domain.OfficeProducts
{
    public class OfficeProductSpecification:Specifications
    {
        public int Id { get; set; }

        public ProductMatrixReferenceType? ProductMatrixReferenceType { get; set; }

        public LetterPaperWeight? LetterPaperWeight { get; set; }
        public LetterColor? LetterColor { get; set; }
        public LetterType? LetterType { get; set; }
        public LetterProcessing? LetterProcessing { get; set; }
        public LetterSize? LetterSize { get; set; }
        public ProductScope? ProductScope { get; set; }
        public int CountryId { get; set; }
        public int ZipId { get; set; }
        public int ContinentId { get; set; }
    }
}
