using LetterAmazer.Business.Services.Domain.Offices;

namespace LetterAmazer.Business.Services.Domain.OfficeProducts
{
    public class ProductMatrixLine
    {
        public ProductMatrixLineType LineType { get; set; }
        public decimal BaseCost { get; set; }
        public string Title { get; set; }
    }
}
