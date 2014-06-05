using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public interface IPartnerInvoiceService
    {
        PartnerInvoice GetPartnerInvoiceById(string accessId,string id);
        List<PartnerInvoice> GetPartnerInvoiceBySpecification(string accessId, PartnerInvoiceSpecification partnerInvoiceSpecification);
    }
}
