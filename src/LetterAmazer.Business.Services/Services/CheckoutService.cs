using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Checkout;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Services
{
    public class CheckoutService:ICheckoutService
    {
        private ICountryService countryService;
        private IPriceService priceService;
        private ICustomerService customerService;
        private IOfficeProductService officeProductService;

        public CheckoutService(ICountryService countryService,IPriceService priceService,
            ICustomerService customerService,
            IOfficeProductService officeProductService)
        {
            this.countryService = countryService;
            this.priceService = priceService;
            this.customerService = customerService;
            this.officeProductService = officeProductService;
        }

        public Order ConvertCheckout(Checkout checkout)
        {
            Order order = new Order();

            Customer customer = customerService.GetCustomerById(checkout.UserId);
            order.Customer = customer;

            Price price = new Price();
            foreach (var letter in checkout.Letters)
            {
                var letterPrice = priceService.GetPriceBySpecification(new PriceSpecification()
                {
                    OfficeProductId = letter.Item1
                });

                order.OrderLines.Add(new OrderLine()
                {
                    BaseProduct = letter.Item2,
                    Price = letterPrice,
                    Quantity = 1,
                    ProductType = ProductType.Letter
                });

                price.AddPrice(letterPrice);
            }

            order.OrderLines.Add(new OrderLine()
            {
                PaymentMethodId = checkout.PaymentMethodId,
                ProductType = ProductType.Payment,
                Price = price
            });

            return order;
        }
    }
}
