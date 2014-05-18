using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Partners
{
    public interface IPartnerService
    {
        Partner GetPartnerById(int id);
        PartnerTransaction GetPartnerTransactionById(int id);
        List<PartnerTransaction> GetPartnerTransactionBySpecification(PartnerTransactionSpecification specification);

    }
}
