using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public interface IPartnerInvoiceService
    {
        PartnerInvoice GetById(string id);
        List<PartnerInvoice> GetBySpecification(PartnerInvoiceSpecification partnerInvoiceSpecification);
    }
}
