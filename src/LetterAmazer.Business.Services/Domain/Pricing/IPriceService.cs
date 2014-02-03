using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Pricing
{
    public interface IPriceService
    {
        List<Price> GetPriceBySpecification(PriceSpecification specification);
    }
}
