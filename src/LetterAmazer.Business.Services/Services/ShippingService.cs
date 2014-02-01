using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Shipping;

namespace LetterAmazer.Business.Services.Services
{
    public class ShippingService:IShippingService
    {
        public List<ShippingService> GetShippingMethods(ShippingMethodSpecification shippingMethodSpecification)
        {
            throw new ArgumentException();
        }
    }
}
