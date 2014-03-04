using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class CustomerFactory : ICustomerFactory
    {
         public ICountryService CountryService { get; set; }

         public CustomerFactory(ICountryService countryService)
        {
            CountryService = countryService;
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
                Credit = dbCustomer.Credits.HasValue ? dbCustomer.Credits.Value : 0.0m,
                CreditLimit = dbCustomer.CreditLimit,
                Password = dbCustomer.Password,
                Phone = dbCustomer.Phone,
                CustomerInfo = new AddressInfo()
                {
                    FirstName = dbCustomer.CustomerInfo_FirstName,
                    LastName = dbCustomer.CustomerInfo_LastName,
                    Address1  = dbCustomer.CustomerInfo_Address,
                    Address2 = dbCustomer.CustomerInfo_Address2,
                    City = dbCustomer.CustomerInfo_City,
                    Zipcode =dbCustomer.CustomerInfo_Zipcode,
                    VatNr = dbCustomer.CustomerInfo_VatNr,
                    Country = dbCustomer.CustomerInfo_Country.HasValue ? CountryService.GetCountryById(dbCustomer.CustomerInfo_Country.Value) : null,
                    AttPerson = dbCustomer.CustomerInfo_AttPerson
                },
                DateActivated = dbCustomer.DateActivated.HasValue ? dbCustomer.DateActivated.Value:(DateTime?)null,
                RegisterKey = dbCustomer.RegistrationKey,
                OrganisationId = dbCustomer.OrganisationId.HasValue ? dbCustomer.OrganisationId.Value : 0
            }; 

            return customer;
        }

        public List<Customer> Create(List<DbCustomers> dbcustomers)
        {
            return dbcustomers.Select(Create).ToList();
        }
    }
}
