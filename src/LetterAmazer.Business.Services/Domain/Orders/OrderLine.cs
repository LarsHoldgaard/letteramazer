using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public class OrderLine
    {
        public ProductType ProductType { get; set; }
        public BaseItem BaseProduct { get; set; }

        public PaymentMethods PaymentMethod { get; set; }
        public int Quantity { get; set; }

        public decimal Cost { get; set; }

        public int CouponId { get; set; }

        public OrderLine()
        {
            this.Quantity = 1;
        }
    }
}
