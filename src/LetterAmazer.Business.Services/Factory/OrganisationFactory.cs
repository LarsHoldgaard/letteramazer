using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OrganisationFactory:IOrganisationFactory
    {
        private IAddressFactory addressFactory;

        public OrganisationFactory(IAddressFactory addressFactory)
        {
            this.addressFactory = addressFactory;
        }

        public Organisation CreateOrganisation(DbOrganisation organisation)
        {
            return new Organisation()
            {
                Id = organisation.Id,
                Guid = organisation.Guid,
                DateCreated = organisation.DateCreated,
                DateDeleted = organisation.DateDeleted,
                DateModified = organisation.DateModified,
                Address = addressFactory.Create(organisation.Address1,organisation.Address2,organisation.Zipcode,
                    organisation.City,organisation.CountryId.Value,organisation.AttPerson,string.Empty,
                    string.Empty,string.Empty,string.Empty,organisation.State,organisation.Name),
              
            };
        }

        public List<Organisation> CreateOrganisation(List<DbOrganisation> organisation)
        {
            return organisation.Select(CreateOrganisation).ToList();
        }
    }
}
