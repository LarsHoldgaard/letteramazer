using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Pricing;

namespace LetterAmazer.Business.Services.Domain.Shipping
{
    public class ShippingMethod
    {
        public DeliveryOption DeliveryOption { get; set; }
        public Price ShippingPrice { get; set; }
        public FulfillmentPartner FulfillmentPartner { get; set; }
    }
}
