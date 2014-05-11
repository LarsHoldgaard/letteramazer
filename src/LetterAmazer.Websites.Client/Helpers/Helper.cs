using System.Collections.Generic;
using System.Configuration;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using iTextSharp.text;
using LetterAmazer.Business.Services.Domain.Countries;

namespace LetterAmazer.Websites.Client.Helpers
{
    public class Helper
    {
        private ICountryService countryService;

        public Helper(ICountryService countryService)
        {
            this.countryService = countryService;
        }


        public void FillCountries(List<SelectListItem> items)
        {
            FillCountries(items,0);
        }
        public void FillCountries(List<SelectListItem> items, int defaultId)
        {
            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            foreach (var country in countries)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.Id.ToString()
                };

                if (defaultId > 0 && country.Id == defaultId)
                {
                    selectedItem.Selected = true;
                }

                items.Add(selectedItem);
            }
        }
    }
}