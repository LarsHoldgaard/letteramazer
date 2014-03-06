using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

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
                Address1 = organisation.Address.Address1,
                Address2 = organisation.Address.Address2,
                City = organisation.Address.City,
                Zipcode = organisation.Address.Zipcode,
                CountryId = organisation.Address.Country.Id,
                State = organisation.Address.State,
                AttPerson = organisation.Address.AttPerson ,
            };

            repository.DbOrganisation.Add(dbOrganisation);
            repository.SaveChanges();

            DbOrganisationProfileSettings dbOrganisationSettings = new DbOrganisationProfileSettings();
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

            var organisation = organisationFactory.Create(dbOrganisation,dbOrganisationSettings);
            organisation.AddressList = addresses;

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
    }
}
