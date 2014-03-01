using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IOrganisationFactory
    {

        Organisation Create(DbOrganisation organisation);
        List<Organisation> Create(List<DbOrganisation> organisation);

        AddressList CreateAddressList(DbOrganisationAddressList list);
        List<AddressList> CreateAddressList(List<DbOrganisationAddressList> list);

    }
}
