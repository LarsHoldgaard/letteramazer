using LetterAmazer.Business.Services.Domain.Currencies;
using LetterAmazer.Business.Services.Domain.OfficeProducts;

namespace LetterAmazer.Business.Services.Domain.ProductMatrix
{
    public class ProductMatrixLine
    {
        public int Id { get; set; }
        public ProductMatrixLineType LineType { get; set; }
        public decimal BaseCost { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string Title { get; set; }

        public int SpanLower { get; set; }
        public int SpanUpper { get; set; }

        public int OfficeProductId { get; set; }

        public ProductMatrixPriceType PriceType { get; set; }
    }
}
