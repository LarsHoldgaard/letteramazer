﻿using System;
using System.Collections.Generic;
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
                ContinentName = country.Continent
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

            return countryFactory.Create(dbcountries.ToList());
        }
    }
}
