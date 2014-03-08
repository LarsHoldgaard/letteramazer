using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
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

        public Organisation Create(DbOrganisation organisation, DbOrganisationProfileSettings organisationProfileSettings)
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
                OrganisationSettings = new OrganisationSettings()
                {
                    Id = organisationProfileSettings.Id,
                    PreferedCountryId  = organisationProfileSettings.PreferedCountryId,
                    LetterColor = (LetterColor?)organisationProfileSettings.LetterColor,
                    LetterPaperWeight = (LetterPaperWeight?)organisationProfileSettings.LetterPaperWeight,
                    LetterProcessing = (LetterProcessing?)organisationProfileSettings.LetterProcessing,
                    LetterSize = (LetterSize?)organisationProfileSettings.LetterSize,
                    LetterType = (LetterType?)organisationProfileSettings.LetterType,

                }
              
            };
        }

        public List<Organisation> Create(List<DbOrganisation> organisation, List<DbOrganisationProfileSettings> organisationProfileSettings)
        {
            if (organisation.Count != organisationProfileSettings.Count)
            {
                throw new ArgumentException("Every organisation needs profile settings");
            }

            return organisation.Select((t, i) => Create(t, organisationProfileSettings[i])).ToList();
        }

        public AddressList CreateAddressList(DbOrganisationAddressList list)
        {
            return new AddressList()
            {
                AddressInfo = addressFactory.Create(list.Address1, list.Address2, list.Zipcode,
                    list.City, list.CountryId,string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, list.State, string.Empty),
                    Id = list.Id,
                    SortIndex = list.OrderIndex
            };
        }

        public List<AddressList> CreateAddressList(List<DbOrganisationAddressList> list)
        {
            return list.Select(CreateAddressList).ToList();
        }
    }
}
