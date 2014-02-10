using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Offices;

namespace LetterAmazer.Business.Services.Domain.OfficeProducts
{
    public class ProductMatrix
    {
        public ProductMatrixPriceType PriceType { get; set; }
        public int SpanLower { get; set; }
        public int SpanUpper { get; set; }
        public List<ProductMatrixLine> ProductLines { get; set; }
    }
}
