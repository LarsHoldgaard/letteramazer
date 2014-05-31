using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class CustomerFactory : ICustomerFactory
    {
        private ICountryService countryService { get; set; }
        private IOrganisationService organsiationService;
        public CustomerFactory(ICountryService countryService, IOrganisationService organsiationService)
        {
            this.countryService = countryService;
            this.organsiationService = organsiationService;
        }

        public Customer Create(DbCustomers dbCustomer)
        {
            var customer = new Customer()
            {
                Id = dbCustomer.Id,
                Email = dbCustomer.Email,
                ResetPasswordKey = dbCustomer.ResetPasswordKey,
                DateCreated = dbCustomer.DateCreated,
                DateModified = dbCustomer.DateUpdated,
                Password = dbCustomer.Password,
                Phone = dbCustomer.Phone,
                CustomerInfo = new AddressInfo()
                {
                    FirstName = dbCustomer.CustomerInfo_FirstName,
                    LastName = dbCustomer.CustomerInfo_LastName,
                    Address1 = dbCustomer.CustomerInfo_Address,
                    Address2 = dbCustomer.CustomerInfo_Address2,
                    City = dbCustomer.CustomerInfo_City,
                    Zipcode = dbCustomer.CustomerInfo_Zipcode,
                    VatNr = dbCustomer.CustomerInfo_VatNr,
                    Country = dbCustomer.CustomerInfo_Country.HasValue ? countryService.GetCountryById(dbCustomer.CustomerInfo_Country.Value) : null,
                    AttPerson = dbCustomer.CustomerInfo_AttPerson
                },
                DateActivated = dbCustomer.DateActivated.HasValue ? dbCustomer.DateActivated.Value : (DateTime?)null,
                RegisterKey = dbCustomer.RegistrationKey,
                OrganisationRole = (OrganisationRole?)dbCustomer.OrganisationRole,
                Organisation = dbCustomer.OrganisationId.HasValue ? organsiationService.GetOrganisationById(dbCustomer.OrganisationId.Value) : null,
                AccountStatus = (AccountStatus)(dbCustomer.AccountStatus)
            };

            return customer;
        }

        public List<Customer> Create(List<DbCustomers> dbcustomers)
        {
            return dbcustomers.Select(Create).ToList();
        }
    }
}
