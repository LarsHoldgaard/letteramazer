using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OfficeFactory
    {
        private ICountryService countryService;

        public OfficeFactory(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        public Office Create(DbOffices dbOffices)
        {
            var office = new Office()
            {
                Title = dbOffices.Name,
                Country = countryService.GetCountryById(dbOffices.CountryId)
            };

            return office;
        }
    }
}
