using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Business.Services.Services.Shipping;

namespace LetterAmazer.Business.Services.Domain.Shipping
{
    public interface IShippingMethodService
    {
        List<ShippingMethod> GetShippingMethodsBySpecification(ShippingMethodSpecification shippingMethodSpecification);
    }
}
