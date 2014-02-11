using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.OrderLines
{
    public class OrderLine
    {
        public ProductType ProductType { get; set; }
        public BaseProduct BaseProduct { get; set; }
        public int Quantity { get; set; }
    }
}
