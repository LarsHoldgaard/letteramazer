using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Checkout;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Files;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Utils.Helpers;
using ProductType = LetterAmazer.Business.Services.Domain.Products.ProductType;

namespace LetterAmazer.Business.Services.Services
{
    public class CheckoutService : ICheckoutService
    {
        private ICountryService countryService;
        private IPriceService priceService;
        private ICustomerService customerService;
        private IOfficeProductService officeProductService;
        private IFileService fileService;

        public CheckoutService(ICountryService countryService, IPriceService priceService,
            ICustomerService customerService,
            IOfficeProductService officeProductService, IFileService fileService)
        {
            this.countryService = countryService;
            this.priceService = priceService;
            this.customerService = customerService;
            this.officeProductService = officeProductService;
            this.fileService = fileService;
        }

        public Order ConvertCheckout(Checkout checkout)
        {
            if (checkout.Letters.Any(c => c.Letter.OfficeId == 0))
            {
                throw new ArgumentException("No letter can have 0 as an officeID on checkout");
            }
            if (checkout.Letters.Any(c => c.OfficeProductId == 0))
            {
                throw new ArgumentException("No letter can have 0 as an officeProductId on checkout");
            }

            Order order = new Order();
            Price price = null;

            fileConversion(checkout);
            setCustomer(checkout, order);

            foreach (var letter in checkout.Letters)
            {
                var letterPrice = priceService.GetPriceBySpecification(new PriceSpecification()
                {
                    OfficeProductId = letter.OfficeProductId,
                    UserId = checkout.UserId,
                    PageCount = letter.Letter.LetterContent.PageCount
                });

                order.OrderLines.Add(new OrderLine()
                {
                    BaseProduct = letter.Letter,
                    Price = letterPrice,
                    Quantity = 1,
                    ProductType = ProductType.Letter
                });

                if (price == null)
                {
                    price = new Price()
                    {
                        PriceExVat = letterPrice.PriceExVat,
                        VatPercentage = letterPrice.VatPercentage
                    };
                }
                else
                {
                    price.AddPrice(letterPrice);
                }

            }

            order.OrderLines.Add(new OrderLine()
            {
                PaymentMethodId = checkout.PaymentMethodId,
                ProductType = ProductType.Payment,
                Price = price
            });

            return order;
        }


        /// <summary>
        /// Finds or create the customer and sets it on the order
        /// </summary>
        /// <param name="checkout"></param>
        /// <param name="order"></param>
        private void setCustomer(Checkout checkout, Order order)
        {
            Customer customer = null;

            // user exists
            if (checkout.UserId > 0)
            {
                customer = customerService.GetCustomerById(checkout.UserId);
            }
            else
            {
                var existingCustomer = customerService.GetCustomerBySpecification(new CustomerSpecification()
                {
                    Email = checkout.Email
                }).FirstOrDefault();

                if (existingCustomer == null)
                {
                    Customer newCustomer = new Customer();
                    newCustomer.Email = checkout.Email;
                    customer = customerService.Create(newCustomer);
                }
                else
                {
                    customer = existingCustomer;
                }
            }
            order.Customer = customer;
        }

        /// <summary>
        /// Convert files into their right filesizes if conversions are needed
        /// </summary>
        /// <param name="checkout"></param>
        private void fileConversion(Checkout checkout)
        {
            foreach (var letter in checkout.Letters)
            {
                // TODO: make a check if the current filesize is different from the file, so we don't make unneeded conversions
                if (letter.Letter.LetterDetails.LetterSize == LetterSize.Letter)
                {
                    var fileData = fileService.Get(letter.Letter.LetterContent.Path);
                    var converted = PdfHelper.ConvertPdfSize(fileData, LetterSize.A4, LetterSize.Letter);
                    fileService.Put(converted, letter.Letter.LetterContent.Path);
                }

            }
        }
    }
}
