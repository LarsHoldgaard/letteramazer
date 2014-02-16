using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OfficeFactory : IOfficeFactory
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
                Country = countryService.GetCountryById(dbOffices.CountryId),
                Id = dbOffices.Id,
                FulfillmentPartnerId = dbOffices.PartnerId
            };

            return office;
        }

        public List<Office> Create(List<DbOffices> dbOfficeses)
        {
            return dbOfficeses.Select(Create).ToList();
        }
    }
}
