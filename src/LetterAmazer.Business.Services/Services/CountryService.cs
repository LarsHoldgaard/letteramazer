using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;

namespace LetterAmazer.Business.Services.Services
{
    public class CountryService : ICountryService
    {
        private IRepository repository;
        private IUnitOfWork unitOfWork;
        public CountryService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public void AddCountry(Country country)
        {
            if (country == null)
            {
                throw new ArgumentException("Country cannot be null");
            }

            repository.Create<Country>(country);
            unitOfWork.Commit();

            // Old import code
            //var jsonData = new StreamReader(Server.MapPath("~/Resources/countryInfoJSON.json")).ReadToEnd();
            //var ababa = JsonConvert.DeserializeObject<Countries>(jsonData);
            //foreach (var countryJson in ababa.geonames)
            //{
            //    countryService.AddCountry(new Country()
            //    {
            //        AreaInSqKm = countryJson.areaInSqKm,
            //        Capital = countryJson.capital,
            //        Continent = countryJson.continent,
            //        ContinentName = countryJson.continentName,
            //        CountryCode = countryJson.countryCode,
            //        CountryName = countryJson.countryName,
            //        CurrencyCode = countryJson.currencyCode,
            //        East = countryJson.east,
            //        FipsCode = countryJson.fipsCode,
            //        GeonameId = countryJson.geonameId,
            //        IsoAlpha3 = countryJson.isoAlpha3,
            //        IsoNumeric = countryJson.isoNumeric,
            //        Languages = countryJson.languages,
            //        North = countryJson.north,
            //        Population = countryJson.population,
            //        South = countryJson.south,
            //        West = countryJson.west
            //    });
            //}

        }

        public Country GetCountry(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id has to be above 0");
            }

            Country country = repository.GetById<Country>(id);
            if (country == null)
            {
                throw new ItemNotFoundException("Country");
            }

            return country;
        }

    }
}
