using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.OfficeProducts;

namespace LetterAmazer.Business.Services.Domain.ProductMatrix
{
    public class ProductMatrix
    {
        public int Id { get; set; }
        public int ValueId { get; set; }
        public ProductMatrixReferenceType ReferenceType { get; set; }
        public ProductMatrixPriceType PriceType { get; set; }
        public int SpanLower { get; set; }
        public int SpanUpper { get; set; }
        public List<ProductMatrixLine> ProductLines { get; set; }
    }
}
