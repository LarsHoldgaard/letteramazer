using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IAddressFactory
    {
        ICountryService CountryService { get; set; }

        AddressInfo Create(string address1, string address2, string postal, string city, int countryid,
            string attPerson, string firstName, string lastName, string vatNr, string co, string state, string organisation);
    }
}