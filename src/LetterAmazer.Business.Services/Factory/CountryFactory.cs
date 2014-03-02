using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class CountryFactory : ICountryFactory
    {
        public Country Create(DbCountries country)
        {
            return new Country()
            {
                Capital = country.Capital,
                ContinentId = country.ContinentId.HasValue ? country.ContinentId.Value : 0,
                InsideEu = country.InsideEu.HasValue && country.InsideEu.Value,
                Name = country.CountryName,
                ArealInSqKm = country.AreaInSqKm,
                CountryCode = country.CountryCode,
                CurrencyCode = country.CurrencyCode,
                Population = country.Population,
                Fipscode = country.FipsCode,
                Id = country.Id,
                VatPercentage = country.VatPercentage
            };
        }

        public List<Country> Create(List<DbCountries> country)
        {
            return country.Select(Create).ToList();
        }

        public Continent CreateContinent(DbContinents dbContinents)
        {
            return new Continent()
            {
                Id = dbContinents.Id,
                Name = dbContinents.Name
            };
        }

        public List<Continent> CreateContinent(List<DbContinents> dbContinents)
        {
            return dbContinents.Select(CreateContinent).ToList();
        }
    }
}
