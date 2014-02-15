using LetterAmazer.Business.Services.Domain.OfficeProducts;

namespace LetterAmazer.Business.Services.Domain.ProductMatrix
{
    public class ProductMatrixLine
    {
        public int Id { get; set; }
        public ProductMatrixLineType LineType { get; set; }
        public decimal BaseCost { get; set; }
        public string Title { get; set; }
    }
}
