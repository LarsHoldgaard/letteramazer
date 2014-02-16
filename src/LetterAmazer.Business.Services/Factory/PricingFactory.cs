using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class PricingFactory : IPricingFactory
    {
        public Pricing Create(DbPricing dbPricing)
        {
            return new Pricing()
            {
                DateCreated = dbPricing.DateCreated,
                DateModified = dbPricing.DateModified,
                Id = dbPricing.Id,
                OfficeProductId = dbPricing.OfficeProductId
            };
        }

        public List<Pricing> Create(List<DbPricing> dbPricings)
        {
            return dbPricings.Select(Create).ToList();
        }
    }
}
