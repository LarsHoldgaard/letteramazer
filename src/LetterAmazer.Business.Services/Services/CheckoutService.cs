using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleWorkflow.Model;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Checkout;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Files;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Utils;
using LetterAmazer.Business.Utils.Helpers;
using ProductType = LetterAmazer.Business.Services.Domain.Products.ProductType;

namespace LetterAmazer.Business.Services.Services
{
    public class CheckoutService : ICheckoutService
    {
        private IPriceService priceService;
        private ICustomerService customerService;
        private IOfficeProductService officeProductService;
        private IFulfillmentPartnerService fulfillmentPartnerService;
        private IFileService fileService;
        private IOfficeService officeService;
        private IOrganisationService organisationService;

        public CheckoutService(IPriceService priceService,ICustomerService customerService,
            IOfficeProductService officeProductService, IFileService fileService, IOrganisationService organisationService, IFulfillmentPartnerService fulfillmentPartnerService, IOfficeService officeService)
        {
            this.priceService = priceService;
            this.customerService = customerService;
            this.officeProductService = officeProductService;
            this.fileService = fileService;
            this.organisationService = organisationService;
            this.fulfillmentPartnerService = fulfillmentPartnerService;
            this.officeService = officeService;
        }

        public Order ConvertCheckout(Checkout checkout)
        {
            if (checkout.CheckoutLines.Any(c => c.ProductType == ProductType.Letter && c.Letter.OfficeId == 0))
            {
                throw new ArgumentException("No letter can have 0 as an officeID on checkout");
            }
            if (checkout.CheckoutLines.Any(c => c.ProductType==ProductType.Letter && c.OfficeProductId == 0))
            {
                throw new ArgumentException("No letter can have 0 as an officeProductId on checkout");
            }
            if (checkout.UserId == 0 && string.IsNullOrEmpty(checkout.Email))
            {
                throw new ArgumentException("A checkout needs either a userID or an email");
            }
            Order order = new Order();
            Price price = null;

            storeFilesCorrectly(checkout);
            fileConversion(checkout);
            setCustomer(checkout, order);

            checkout.OrderNumber = Helpers.GetRandomInt(1000, 99999999).ToString();

            Customer customer = order.Customer;

            foreach (var letter in checkout.CheckoutLines)
            {
                
                if (letter.ProductType == ProductType.Credit)
                {
                    var costPrice = new Price()
                    {
                        PriceExVat = letter.Quantity,
                        VatPercentage = customer.VatPercentage()
                    };
                    order.OrderLines.Add(new OrderLine()
                    {
                        ProductType = ProductType.Credit,
                        Quantity = letter.Quantity,
                        Price = costPrice
                    });
                    price = setPriceOnLine(price, costPrice);
                }

                if (letter.ProductType == ProductType.Letter)
                {
                    var officeProduct = officeProductService.GetOfficeProductById(letter.OfficeProductId);
                   
                    var letterPrice = priceService.GetPriceBySpecification(new PriceSpecification()
                    {
                        OfficeProductId = letter.OfficeProductId,
                        UserId = customer.Id,
                        PageCount = letter.Letter.LetterContent.PageCount
                    });

                    order.OrderLines.Add(new OrderLine()
                    {
                        BaseProduct = letter.Letter,
                        Price = letterPrice,
                        Quantity = 1,
                        ProductType = ProductType.Letter
                    });

                    price = setPriceOnLine(price,letterPrice);

                    letter.Letter.ReturnLabel = Helpers.GetRandomInt(1000000, 99999999);
                    letter.Letter.DeliveryLabel = officeProduct.DeliveryLabel;
                    letter.Letter.CustomerId = customer.Id;
                    letter.Letter.OrganisationId = customer.Organisation.Id;
                }

            }

            order.OrderCode = checkout.OrderNumber;
            order.OrganisationId = customer.Organisation.Id;

            order.OrderLines.Add(new OrderLine()
            {
                PaymentMethodId = checkout.PaymentMethodId,
                ProductType = ProductType.Payment,
                Price = price
            });

            order.PartnerTransactions = checkout.PartnerTransactions;
            setReturnPostId(checkout);
            setCustomNumbers(checkout);
            return order;
        }

        private void storeFilesCorrectly(Checkout checkout)
        {
            foreach (var checkoutLine in checkout.CheckoutLines)
            {
                if (checkoutLine.ProductType == ProductType.Letter)
                {
                    var byteData = fileService.GetFileById(checkoutLine.Letter.LetterContent.Path,FileUploadMode.Temporarily);
                    var keyName = fileService.Create(byteData,
                        Helpers.GetUploadDateString(Guid.NewGuid().ToString()));

                    checkoutLine.Letter.LetterContent.Path = keyName;
                }
            }
        }

        private Price setPriceOnLine(Price price, Price letterPrice)
        {
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
            return price;
        }

        /// <summary>
        /// Prints the letterID on the PDF, so we can mark it as return post
        /// </summary>
        /// <param name="checkout"></param>
        private void setReturnPostId(Checkout checkout)
        {
            foreach (var letter in checkout.CheckoutLines.Where(c=>c.ProductType== ProductType.Letter))
            {
                var fileData = fileService.GetFileById(letter.Letter.LetterContent.Path);
                var converted = PdfHelper.WriteIdOnPdf(fileData, letter.Letter.ReturnLabel.ToString());
                fileService.Create(converted, letter.Letter.LetterContent.Path);
            }
        }

        private void setCustomNumbers(Checkout checkout)
        {
            foreach (var letter in checkout.CheckoutLines.Where(c => c.ProductType == ProductType.Letter))
            {
                var officeProduct = officeProductService.GetOfficeProductById(letter.OfficeProductId);
                var office = officeService.GetOfficeById(officeProduct.OfficeId);
                var fulfillment = fulfillmentPartnerService.GetFulfillmentPartnerById(office.FulfillmentPartnerId);

                if (fulfillment.PartnerJob == PartnerJob.Handikuvertering)
                {
                    var fileData = fileService.GetFileById(letter.Letter.LetterContent.Path);
                    var converted = PdfHelper.WriteSameStrOnAllPages(fileData, letter.Letter.ReturnLabel.ToString());
                    fileService.Create(converted, letter.Letter.LetterContent.Path);   
                }
            }
            
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

                    Organisation organisation =new Organisation()
                    {
                        IsPrivate = true,
                        Name = checkout.Email,
                        Address = new AddressInfo()
                        {
                            Country = new Country() { Id = checkout.OriginCountry}
                        }
                    };
                    var organisation_stored = organisationService.Create(organisation);
                    newCustomer.Organisation = organisation_stored;

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
            foreach (var letter in checkout.CheckoutLines.Where(c=>c.ProductType == ProductType.Letter))
            {
                // TODO: make a check if the current filesize is different from the file, so we don't make unneeded conversions
                if (letter.Letter.LetterDetails.LetterSize == LetterSize.Letter)
                {
                    var fileData = fileService.GetFileById(letter.Letter.LetterContent.Path);
                    var converted = PdfHelper.ConvertPdfSize(fileData, LetterSize.A4, LetterSize.Letter);
                    fileService.Create(converted, letter.Letter.LetterContent.Path);
                }

            }
        }

        

    }
}
