using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class FulfilmentPartnerFactory
    {
        public FulfilmentPartner Create(DbFulfillmentPartners dbpFulfillmentPartners)
        {
            return new FulfilmentPartner()
            {
                Id = dbpFulfillmentPartners.Id,
                Name = dbpFulfillmentPartners.Name
            };
        }
    }
}
