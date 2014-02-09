using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Factory.Interfaces;

namespace LetterAmazer.Business.Services.Factory
{
    public class AddressFactory : IAddressFactory
    {
        public ICountryService CountryService { get; set; }

        public AddressFactory(ICountryService countryService)
        {
            CountryService = countryService;
        }

        public AddressInfo Create(string address1, string address2, string postal, string city, int countryid,
            string attPerson, string firstName, string lastName, string vatNr, string co, string state)
        {
            var country = CountryService.GetCountryById(countryid);

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
