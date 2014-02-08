using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class CountryFactory
    {
        public Country Create(DbCountries country)
        {
            return new Country()
            {
                Capital = country.Capital,
                Continent = country.Continent,
                InsideEu = country.InsideEu.HasValue && country.InsideEu.Value,
                Name = country.ContinentName,
                ArealInSqKm = country.AreaInSqKm,
                CountryCode = country.CountryCode,
                CurrencyCode = country.CurrencyCode,
                Population = country.Population,
                Fipscode = country.FipsCode,
                Id = country.Id
            };
        }
    }
}
