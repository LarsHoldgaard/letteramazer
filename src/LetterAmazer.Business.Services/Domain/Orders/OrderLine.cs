using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public class OrderLine
    {
        public ProductType ProductType { get; set; }
        public BaseProduct BaseProduct { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }

        public decimal Cost { get; set; }

        public OrderLine()
        {
            this.Quantity = 1;
        }
    }
}
