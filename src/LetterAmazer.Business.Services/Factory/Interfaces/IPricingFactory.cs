using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IPricingFactory
    {
        Pricing Create(DbPricing dbPricing);
        List<Pricing> Create(List<DbPricing> dbPricings);
    }
}