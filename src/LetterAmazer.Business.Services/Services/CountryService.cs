using System;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CountryService : ICountryService
    {
        private LetterAmazerEntities repository;
        private CountryFactory countryFactory;

        public CountryService(LetterAmazerEntities repository)
        {
            this.repository = repository;
        }

        public void AddCountry(Country country)
        {
            if (country == null)
            {
                throw new ArgumentException("Country cannot be null");
            }

            repository.DbCountries.Add(new DbCountries()
            {
                Capital = country.Capital,
                CountryName = country.Name,
                Continent = country.Capital
            });
            repository.SaveChanges();
        }

        public Country GetCountryById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id has to be above 0");
            }

            var country = repository.DbCountries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                throw new ItemNotFoundException("Country");
            }

            return countryFactory.Create(country);
        }

    }
}
