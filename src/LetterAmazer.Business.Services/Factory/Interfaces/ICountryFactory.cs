using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface ICountryFactory
    {
        Country Create(DbCountries country);
        List<Country> Create(List<DbCountries> country);
        Continent CreateContinent(DbContinents dbContinents);
        List<Continent> CreateContinent(List<DbContinents> dbContinents);

        CountryName CreateCountryName(DbCountryNames countryNames);
        List<CountryName> CreateCountryName(List<DbCountryNames> countryNames);

    }
}