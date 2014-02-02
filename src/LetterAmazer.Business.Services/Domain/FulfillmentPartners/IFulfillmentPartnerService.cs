using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Fulfillments;

namespace LetterAmazer.Business.Services.Domain.FulfillmentPartners
{
    public interface IFulfillmentPartnerService
    {
        List<FulfillmentPartner> GetFulfillmentPartnersBySpecifications(FulfillmentPartnerSpecification specification);
        FulfillmentPartner GetFulfillmentPartnerById(int id);
    }
}
