using System.Collections.Generic;
using System.Configuration;
using System.Web.Http.ModelBinding;
using System.Web.Mvc;
using iTextSharp.text;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Websites.Client.Helpers
{
    public class Helper
    {
        public static void FillCountries(ICountryService countryService, List<SelectListItem> items)
        {
            FillCountries(countryService, items, 0);
        }

        public static void FillCountries(ICountryService countryService, List<SelectListItem> items, int defaultId)
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

        public static void FillPaymentMethods(IPaymentService paymentService, List<SelectListItem> items, PaymentType paymentType)
        {
            int customerId = 0;
            if (SessionHelper.Customer != null)
            {
                customerId = SessionHelper.Customer.Id;
            }

            var possiblePaymentMethods =
           paymentService.GetPaymentMethodsBySpecification(new PaymentMethodSpecification()
           {
               CustomerId = customerId,
               PaymentType = paymentType
           });

            foreach (var possiblePaymentMethod in possiblePaymentMethods)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = possiblePaymentMethod.Label,
                    Value = possiblePaymentMethod.Id.ToString()
                };
                items.Add(selectedItem);
            }
        }
    }
}