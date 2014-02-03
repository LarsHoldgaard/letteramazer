using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Address;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Services;

namespace LetterAmazer.Business.Services.Factory
{
    public class AddressFactory
    {
        public ICountryService CountryService { get; set; }

        public AddressFactory(ICountryService countryService)
        {
            CountryService = countryService;
        }

        public AddressInfo Create(string address1, string address2, string postal, string city, int countryid,
            string attPerson, string firstName, string lastName, string vatNr)
        {
            var country = CountryService.GetCountry(countryid);

            return new AddressInfo()
            {
                Address1 = address1,
                Address2 = address2,
                AttPerson = attPerson,
                City = city,
                Country = country,
                FirstName = firstName,
                LastName = lastName,
                PostalCode = postal,
                VatNr = vatNr
            };
        }
    }
}
