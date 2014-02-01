using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Services;

namespace LetterAmazer.Business.Services.Domain.Shipping
{
    public interface IShippingService
    {
        List<ShippingService> GetShippingMethods(ShippingMethodSpecification shippingMethodSpecification);
    }
}
