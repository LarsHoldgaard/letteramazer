using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Services
{
    public class PriceService:IPriceService
    {
        private ICountryService countryService;

        public PriceService(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        public Price GetPriceByOrder(Order order)
        {
            var price = new Price();
            foreach (var orderLine in order.OrderLines)
            {
                if (orderLine.ProductType == ProductType.Order)
                {
                    var letter = (Letter) orderLine.BaseProduct;
                    var letterPrice = GetPriceByLetter(letter);
                    price.PriceExVat += letterPrice.PriceExVat;
                    price.VatPercentage = letterPrice.PriceExVat;    
                }
                
            }
            return price;
        }

        public Price GetPriceByLetter(Letter letter)
        {
            return GetPriceByAddress(letter.ToAddress);
        }

        public Price GetPriceByAddress(AddressInfo addressInfo)
        {
            var country = countryService.GetCountryById(addressInfo.Country.Id);

            return new Price()
            {
                PriceExVat = 1.0m,
                VatPercentage = 0.25m
            };
        }
    }
}
