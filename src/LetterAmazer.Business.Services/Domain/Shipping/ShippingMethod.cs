using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Pricing;

namespace LetterAmazer.Business.Services.Domain.Shipping
{
    public class ShippingMethod
    {
        public DeliveryOption DeliveryOption { get; set; }
        public Price ShippingPrice { get; set; }
        public Office Office { get; set; }
    }
}
