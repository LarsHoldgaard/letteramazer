using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface ICountryFactory
    {
        Country Create(DbCountries country);
        List<Country> Create(List<DbCountries> country);
    }
}