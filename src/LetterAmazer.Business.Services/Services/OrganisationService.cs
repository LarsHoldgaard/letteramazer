using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;
using LetterAmazer.Business.Services.Domain.Api;

namespace LetterAmazer.Business.Services.Services
{
    public class OrganisationService : IOrganisationService
    {
        private IOrganisationFactory organisationFactory;
        private LetterAmazerEntities repository;

        public OrganisationService(LetterAmazerEntities repository, IOrganisationFactory organisationFactory)
        {
            this.repository = repository;
            this.organisationFactory = organisationFactory;
        }

        public Organisation Create(Organisation organisation)
        {
            var dbOrganisation = new DbOrganisation()
            {
                Guid = Guid.NewGuid(),
                Name = organisation.Name,
                DateCreated = DateTime.Now,
                IsPrivate = organisation.IsPrivate,
                CountryId = organisation.Address.Country.Id
            };

            if (!organisation.IsPrivate)
            {
                dbOrganisation.Address1 = organisation.Address.Address1;
                dbOrganisation.Address2 = organisation.Address.Address2;
                dbOrganisation.City = organisation.Address.City;
                dbOrganisation.Zipcode = organisation.Address.Zipcode;
                dbOrganisation.CountryId = organisation.Address.Country.Id;
                dbOrganisation.State = organisation.Address.State;
                dbOrganisation.AttPerson = organisation.Address.AttPerson;
            }

            repository.DbOrganisation.Add(dbOrganisation);
            repository.SaveChanges();

            DbOrganisationProfileSettings dbOrganisationSettings = new DbOrganisationProfileSettings();
            dbOrganisationSettings.LetterType = (int)LetterType.Pres;
            dbOrganisationSettings.OrganisationId = dbOrganisation.Id;

            repository.DbOrganisationProfileSettings.Add(dbOrganisationSettings);
            repository.SaveChanges();

            return GetOrganisationById(dbOrganisation.Id);
        }

        public Organisation Update(Organisation organisation)
        {
            var dbOrganisation = repository.DbOrganisation.FirstOrDefault(c => c.Id == organisation.Id);

            if (dbOrganisation == null)
            {
                throw new ArgumentException("Item doesn't exist with ID: " + organisation.Id);
            }

            dbOrganisation.Name = organisation.Name;
            dbOrganisation.DateModified = DateTime.Now;
            dbOrganisation.Address1 = organisation.Address.Address1;
            dbOrganisation.Address2 = organisation.Address.Address2;
            dbOrganisation.State = organisation.Address.State;
            dbOrganisation.City = organisation.Address.City;
            dbOrganisation.AttPerson = organisation.Address.AttPerson;
            dbOrganisation.Zipcode = organisation.Address.Zipcode;
            dbOrganisation.CountryId = organisation.Address.Country.Id;
            dbOrganisation.IsPrivate = organisation.IsPrivate;

            var dbOrganisationSettings =
                repository.DbOrganisationProfileSettings.FirstOrDefault(c => c.OrganisationId == organisation.Id);

            if (dbOrganisationSettings == null)
            {
                throw new ArgumentException("No organisation settings with ID: " + organisation.Id);
            }

            dbOrganisationSettings.PreferedCountryId = organisation.OrganisationSettings.PreferedCountryId;
            dbOrganisationSettings.LetterPaperWeight = (int?)organisation.OrganisationSettings.LetterPaperWeight;
            dbOrganisationSettings.LetterColor = (int?)organisation.OrganisationSettings.LetterColor;
            dbOrganisationSettings.LetterProcessing = (int?)organisation.OrganisationSettings.LetterProcessing;
            dbOrganisationSettings.LetterSize = (int?)organisation.OrganisationSettings.LetterSize;
            dbOrganisationSettings.LetterType = (int?)organisation.OrganisationSettings.LetterType;

            repository.SaveChanges();

            return GetOrganisationById(organisation.Id);
        }

        public void Delete(Organisation organisation)
        {
            var dbOrganisation = repository.DbOrganisation.FirstOrDefault(c => c.Id == organisation.Id);

            if (dbOrganisation == null)
            {
                throw new ArgumentException("No organisation settings with ID: " + organisation.Id);
            }

            repository.DbOrganisation.Remove(dbOrganisation);
            repository.SaveChanges();
        }

        public Organisation GetOrganisationById(int id)
        {
            var dbOrganisation = repository.DbOrganisation.FirstOrDefault(c => c.Id == id);


            if (dbOrganisation == null)
            {
                throw new ArgumentException("Item doesn't exist with ID: " + id);
            }

            var dbOrganisationSettings =
              repository.DbOrganisationProfileSettings.FirstOrDefault(c => c.OrganisationId == id);

            if (dbOrganisationSettings == null)
            {
                throw new ArgumentException("No organisation settings with ID: " + id);
            }

            var dbAddresses = repository.DbOrganisationAddressList.Where(c => c.OrganisationId == id);
            var addresses = organisationFactory.CreateAddressList(dbAddresses.ToList());

            var dbApis = repository.DbApiAccess.Where(c => c.OrganisationId == id);
            var apis = organisationFactory.CreateApiKeys(dbApis.ToList());

            var organisation = organisationFactory.Create(dbOrganisation, dbOrganisationSettings);
            organisation.AddressList = addresses;
            organisation.ApiKeys = apis;

            return organisation;
        }

        public AddressList GetAddressListById(int id)
        {
            var dbAddressList = repository.DbOrganisationAddressList.FirstOrDefault(c => c.Id == id);

            if (dbAddressList == null)
            {
                throw new ArgumentException("No address info for this ID: " + id);
            }

            var addressList = organisationFactory.CreateAddressList(dbAddressList);
            return addressList;
        }

        public AddressList Update(AddressList addressList)
        {
            var dbaddresslist = repository.DbOrganisationAddressList.FirstOrDefault(c => c.Id == addressList.Id);
            dbaddresslist.Address1 = addressList.AddressInfo.Address1;
            dbaddresslist.Address2 = addressList.AddressInfo.Address2;
            dbaddresslist.City = addressList.AddressInfo.City;
            dbaddresslist.Zipcode = addressList.AddressInfo.Zipcode;
            dbaddresslist.OrderIndex = addressList.SortIndex;
            dbaddresslist.CountryId = addressList.AddressInfo.Country.Id;
            dbaddresslist.OrganisationId = addressList.OrganisationId;
            dbaddresslist.State = addressList.AddressInfo.State;
            dbaddresslist.OrganisationName = addressList.AddressInfo.Organisation;

            repository.SaveChanges();

            return GetAddressListById(addressList.Id);
        }

        public AddressList Create(AddressList addressList)
        {
            var dbAddresslist = new DbOrganisationAddressList()
            {
                Address1 = addressList.AddressInfo.Address1,
                Address2 = addressList.AddressInfo.Address2,
                City = addressList.AddressInfo.City,
                Zipcode = addressList.AddressInfo.Zipcode,
                OrderIndex = addressList.SortIndex,
                CountryId = addressList.AddressInfo.Country.Id,
                OrganisationId = addressList.OrganisationId,
                State = addressList.AddressInfo.State,
                OrganisationName = addressList.AddressInfo.Organisation
            };

            repository.DbOrganisationAddressList.Add(dbAddresslist);
            repository.SaveChanges();

            return GetAddressListById(dbAddresslist.Id);

        }

        /// <summary>
        /// Get API Key details
        /// </summary>
        /// <param name="apiKey"> api key</param>
        /// <param name="apiSecreat">api secreat key</param>
        /// <returns></returns>
        public ApiAccess GetApiKeys(string apiKey, string apiSecreat)
        {
            ApiAccess apiAccess = null;
            var AccessList = repository.DbApiAccess.Where(q => q.ApiKey == apiKey && q.ApiSecret == apiSecreat);
            if (AccessList != null && AccessList.Count() > 0)
            {
                apiAccess = new ApiAccess();
                var dbApiAccess = AccessList.FirstOrDefault();
                apiAccess.ApiKey = dbApiAccess.ApiKey;
                apiAccess.ApiSecret = dbApiAccess.ApiSecret;
                apiAccess.OrganisationId = dbApiAccess.OrganisationId;
                if (dbApiAccess.Role != null && dbApiAccess.Role > 0)
                {
                    apiAccess.Role = (Role)dbApiAccess.Role;
                }
                else
                {
                    apiAccess.Role = Role.NONE;
                }
            }
            return apiAccess;
        } 
    }
}
