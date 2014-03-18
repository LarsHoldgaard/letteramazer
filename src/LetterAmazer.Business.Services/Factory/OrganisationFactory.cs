using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Api;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OrganisationFactory : IOrganisationFactory
    {
        private IAddressFactory addressFactory;

        public OrganisationFactory(IAddressFactory addressFactory)
        {
            this.addressFactory = addressFactory;
        }

        public Organisation Create(DbOrganisation organisation, DbOrganisationProfileSettings organisationProfileSettings)
        {
            var new_organisation= new Organisation()
            {
                Id = organisation.Id,
                Guid = organisation.Guid,
                DateCreated = organisation.DateCreated,
                DateDeleted = organisation.DateDeleted,
                DateModified = organisation.DateModified,
                IsPrivate = organisation.IsPrivate,
                Name = organisation.Name,
               RequiredFulfillmentPartnerId = organisation.RequiredFulfillmentPartnerId,
               RequiredOfficeId = organisation.RequiredOfficeId
            };

            int countryId = 0;
            if (organisation.CountryId.HasValue && organisation.CountryId.Value > 0)
            {
                countryId = organisation.CountryId.Value;
            }

            new_organisation.Address = addressFactory.Create(organisation.Address1, organisation.Address2,
                organisation.Zipcode,
                organisation.City, countryId, organisation.AttPerson, string.Empty,
                string.Empty, string.Empty, string.Empty, organisation.State, organisation.Name);

            new_organisation.OrganisationSettings = new OrganisationSettings()
            {
                Id = organisationProfileSettings.Id,
                PreferedCountryId = organisationProfileSettings.PreferedCountryId,
                LetterColor = (LetterColor?) organisationProfileSettings.LetterColor,
                LetterPaperWeight = (LetterPaperWeight?) organisationProfileSettings.LetterPaperWeight,
                LetterProcessing = (LetterProcessing?) organisationProfileSettings.LetterProcessing,
                LetterSize = (LetterSize?) organisationProfileSettings.LetterSize,
                LetterType = (LetterType?) organisationProfileSettings.LetterType,

            };

            return new_organisation;
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
                    list.City, list.CountryId, string.Empty, string.Empty,
                    string.Empty, string.Empty, string.Empty, list.State, list.OrganisationName),
                Id = list.Id,
                SortIndex = list.OrderIndex,
                OrganisationId = list.OrganisationId
            };
        }

        public List<AddressList> CreateAddressList(List<DbOrganisationAddressList> list)
        {
            return list.Select(CreateAddressList).ToList();
        }

        public ApiKeys CreateApiKeys(DbApiAccess apiAccess)
        {
            return new ApiKeys()
            {
                ApiKey = apiAccess.ApiKey,
                ApiSecret = apiAccess.ApiSecret
            };
        }

        public List<ApiKeys> CreateApiKeys(List<DbApiAccess> apiAccess)
        {
            return apiAccess.Select(CreateApiKeys).ToList();
        }
    }
}
