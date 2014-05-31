using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Partners;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IPartnerFactory
    {
        Partner Create(DbPartners partners);
        List<Partner> Create(List<DbPartners> partners);

        PartnerTransaction Create(DbPartnerTransactions partners);
        List<PartnerTransaction> Create(List<DbPartnerTransactions> partners);


    }
}
