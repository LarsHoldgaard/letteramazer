using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Pricing
{
    public interface IPriceService
    {
        Price GetPriceByOrder(Order order);
        Price GetPriceByLetter(Letter letter);
        Price GetPriceByAddress(AddressInfo addressInfo);
        Price GetPriceBySpecification(PriceSpecification specification);
        Pricing Create(Pricing pricing);
    }
}
