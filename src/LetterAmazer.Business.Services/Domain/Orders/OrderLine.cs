using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public class OrderLine
    {
        public ProductType ProductType { get; set; }
        public BaseItem BaseProduct { get; set; }

     
        public int PaymentMethodId { get; set; }

        public int Quantity { get; set; }

        public Price Price { get; set; }
        public int CouponId { get; set; }


        public OrderLine()
        {
            this.Quantity = 1;
        }
    }
}
