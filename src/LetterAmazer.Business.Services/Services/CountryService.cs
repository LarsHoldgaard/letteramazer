using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class CountryService : ICountryService
    {
        private LetterAmazerEntities repository;
        private ICountryFactory countryFactory;

        public CountryService(LetterAmazerEntities repository, ICountryFactory countryFactory)
        {
            this.repository = repository;
            this.countryFactory = countryFactory;
        }

        public Country Create(Country country)
        {
            if (country == null)
            {
                throw new ArgumentException("Country cannot be null");
            }

            repository.DbCountries.Add(new DbCountries()
            {
                Capital = country.Capital,
                CountryName = country.Name,
                Continent = country.Capital,
                InsideEu = country.InsideEu,
                AreaInSqKm = country.ArealInSqKm,
                ContinentId = country.ContinentId,
            });
            repository.SaveChanges();

            return GetCountryById(country.Id);
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

        public List<Country> GetCountryBySpecificaiton(CountrySpecification specification)
        {
            IQueryable<DbCountries> dbcountries = repository.DbCountries;

            if (specification.Id > 0)
            {
                dbcountries = repository.DbCountries.Where(c => c.Id == specification.Id);
            }
            if (specification.InsideEu != null)
            {
                dbcountries = repository.DbCountries.Where(c => c.InsideEu.Value);
            }
            if (!string.IsNullOrEmpty(specification.CountryCode))
            {
                dbcountries = repository.DbCountries.Where(c => c.CountryCode == specification.CountryCode);
            }
            if (!string.IsNullOrEmpty(specification.CountryName))
            {
                dbcountries = repository.DbCountries.Where(c => c.CountryName == specification.CountryName);
            }
            if (specification.ContinentId > 0)
            {
                dbcountries = repository.DbCountries.Where(c => c.ContinentId == specification.ContinentId);
            }
            return countryFactory.Create(dbcountries.OrderBy(c=>c.CountryName).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public List<Continent> GetContinents()
        {
            var dbContinent = repository.DbContinents.ToList();

            return countryFactory.CreateContinent(dbContinent);
        }

        public CountryName Create(CountryName countryName)
        {
            var dbCountry = new DbCountryNames()
            {
                CountryId = countryName.CountryId,
                Name = countryName.Name,
                Language = countryName.Language
            };

            repository.DbCountryNames.Add(dbCountry);
            repository.SaveChanges();

            return GetCountryNameById(dbCountry.Id);
        }

        public CountryName GetCountryNameById(int id)
        {
            var dbCountryName = repository.DbCountryNames.First(c => c.Id == id);

            if (dbCountryName == null)
            {
                throw new ArgumentException("No countryname with this id: " + id);
            }

            return countryFactory.CreateCountryName(dbCountryName);
        }

        public List<CountryName> GetCountryNamesBySpecification(CountryNameSpecification specification)
        {
            IQueryable<DbCountryNames> dbCustomers = repository.DbCountryNames;

            return countryFactory.
                CreateCountryName(dbCustomers.OrderBy(c=>c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }
    }
}
