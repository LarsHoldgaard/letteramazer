using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class FulfilmentPartnerFactory : IFulfilmentPartnerFactory
    {
        public FulfilmentPartner Create(DbFulfillmentPartners dbpFulfillmentPartners)
        {
            return new FulfilmentPartner()
            {
                Id = dbpFulfillmentPartners.Id,
                Name = dbpFulfillmentPartners.Name,
                ShopId = dbpFulfillmentPartners.ShopId,
                PartnerJob = (PartnerJob)dbpFulfillmentPartners.PartnerJob,
                CronInterval = dbpFulfillmentPartners.CronInterval
            };
        }

        public List<FulfilmentPartner> Create(List<DbFulfillmentPartners> dbFulfillmentPartnerses)
        {
            return dbFulfillmentPartnerses.Select(Create).ToList();
        }
    }
}
