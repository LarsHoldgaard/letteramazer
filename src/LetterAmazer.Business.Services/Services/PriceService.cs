using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Pricing;

namespace LetterAmazer.Business.Services.Services
{
    public class PriceService:IPriceService
    {
        public List<Price> GetPriceBySpecification(PriceSpecification specification)
        {
            return new List<Price>()
            {
                new Price()
                {
                    PriceExVat = 1.0m,
                    VatPercentage = 0.25m
                }
            };
        }
    }
}
