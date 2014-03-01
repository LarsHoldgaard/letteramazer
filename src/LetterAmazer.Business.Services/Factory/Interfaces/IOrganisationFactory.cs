using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IOrganisationFactory
    {

        Organisation CreateOrganisation(DbOrganisation organisation);
        List<Organisation> CreateOrganisation(List<DbOrganisation> organisation);
    }
}
