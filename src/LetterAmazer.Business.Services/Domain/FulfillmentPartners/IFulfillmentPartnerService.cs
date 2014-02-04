using System.Collections.Generic;

namespace LetterAmazer.Business.Services.Domain.FulfillmentPartners
{
    public interface IFulfillmentPartnerService
    {
        List<FulfilmentPartner> GetFulfillmentPartnersBySpecifications(FulfillmentPartnerSpecification specification);
        FulfilmentPartner GetFulfillmentPartnerById(int id);
    }
}
