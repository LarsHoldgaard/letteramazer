using System.Collections.Generic;
using iTextSharp.text;

namespace LetterAmazer.Business.Services.Domain.Countries
{
    public interface ICountryService
    {

        Country Create(Country country);
        Country GetCountryById(int id);
        List<Country> GetCountryBySpecificaiton(CountrySpecification specification);
    }
}
