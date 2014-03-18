using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IFulfilmentPartnerFactory
    {
        FulfilmentPartner Create(DbFulfillmentPartners dbpFulfillmentPartners);
        List<FulfilmentPartner> Create(List<DbFulfillmentPartners> dbFulfillmentPartnerses);
    }
}