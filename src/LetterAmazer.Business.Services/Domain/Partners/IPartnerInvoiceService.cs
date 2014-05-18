using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public interface IPartnerInvoiceService
    {
        PartnerInvoice GetPartnerInvoiceById(string id);
        List<PartnerInvoice> GetPartnerInvoiceBySpecification(PartnerInvoiceSpecification partnerInvoiceSpecification);
    }
}
