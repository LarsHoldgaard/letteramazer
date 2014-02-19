using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Data.Repository.Data;
using ProductType = LetterAmazer.Business.Services.Domain.Products.ProductType;

namespace LetterAmazer.Business.Services.Services
{
    public class PriceService : IPriceService
    {
        private ICountryService countryService;
        private IOrderLineService orderLineService;
        private LetterAmazerEntities repository;
        private IProductMatrixService productMatrixService;
        private IOfficeProductService offerProductService;

        public PriceService(ICountryService countryService, IOrderLineService orderLineService,
            LetterAmazerEntities repository, IProductMatrixService productMatrixService, IOfficeProductService offerProductService)
        {
            this.countryService = countryService;
            this.orderLineService = orderLineService;
            this.repository = repository;
            this.productMatrixService = productMatrixService;
            this.offerProductService = offerProductService;
        }

        public Price GetPriceByOrder(Order order)
        {
            var lines = orderLineService.GetOrderlineBySpecification(new OrderLineSpecification() { OrderId = order.Id });
            var price = new Price();
            foreach (var orderLine in lines)
            {
                if (orderLine.ProductType == ProductType.Order)
                {
                    var letter = (Letter)orderLine.BaseProduct;
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
            if (addressInfo.Country == null || addressInfo.Country.Id ==0)
            {
                throw new BusinessException("Country cannot be null of an address");
            }

            var price = GetPriceBySpecification(new PriceSpecification()
            {
                CountryId = addressInfo.Country.Id,
                LetterColor = LetterColor.Color,
                LetterProcessing = LetterProcessing.Dull,
                LetterSize = LetterSize.A4,
                LetterType = LetterType.Pres,
                LetterPaperWeight = LetterPaperWeight.Eight
            });

            return price;
        }

        public Price GetPriceBySpecification(PriceSpecification specification)
        {
            
            var pricing = from d in repository.DbPricing
                           join d1 in repository.DbOfficeProducts on d.OfficeProductId equals d1.Id
                           join d2 in repository.DbOfficeProductDetails on d1.ProductDetailsId equals d2.Id
                           select new PriceRetrival()
                           {
                                PricingId = d.Id,
                                LetterColor = (LetterColor)d2.LetterColor,
                                LetterPaperWeight = (LetterPaperWeight)d2.LetterPaperWeight,
                                LetterProcessing = (LetterProcessing)d2.LetterProcessing,
                                LetterType = (LetterType)d2.LetterType,
                                LetterSize = (LetterSize)d2.LetterSize,
                                OfficeProductId = d1.Id,
                                OfficeDetailId = d2.Id,
                                CountryId = d1.CountryId.HasValue ? d1.CountryId.Value : 0,
                                ZipId = d1.ZipId.HasValue ? d1.ZipId.Value : 0,
                                ContinentId = d1.ContinentId.HasValue ? d1.ContinentId.Value : 0,
                                OfficeId = d1.OfficeId,
                                ProductScope = (ProductScope)d1.ScopeType
                           };

            if (specification.CountryId > 0)
            {
                var country = countryService.GetCountryById(specification.CountryId);

                pricing = pricing.Where(c => c.CountryId == specification.CountryId || 
                    (c.ProductScope == ProductScope.RestOfWorld) ||
                    (c.ProductScope == ProductScope.Continent && country.ContinentId == c.ContinentId));
            }
            if (specification.ContinentId > 0)
            {
                pricing = pricing.Where(c => c.ContinentId == specification.ContinentId);
            }
            if (specification.OfficeId > 0)
            {
                pricing = pricing.Where(c => c.OfficeId == specification.OfficeId);
            }
            if (specification.ZipId > 0)
            {
                pricing = pricing.Where(c => c.ZipId == specification.ZipId);
            }
            if (specification.LetterColor.HasValue)
            {
                pricing = pricing.Where(c => c.LetterColor == specification.LetterColor.Value);
            }
            if (specification.LetterPaperWeight.HasValue)
            {
                pricing = pricing.Where(c => c.LetterPaperWeight == specification.LetterPaperWeight.Value);
            }
            if (specification.LetterSize.HasValue)
            {
                pricing = pricing.Where(c => c.LetterSize == specification.LetterSize.Value);
            }
            if (specification.LetterType.HasValue)
            {
                pricing = pricing.Where(c => c.LetterType == specification.LetterType.Value);
            }
            if (specification.LetterProcessing.HasValue)
            {
                pricing = pricing.Where(c => c.LetterProcessing == specification.LetterProcessing.Value);
            }

            var allPrices = pricing.ToList();

            int officeProductId = 0;
            decimal minSum = decimal.MaxValue;
            foreach (var priceRetrival in allPrices)
            {
                var lines = productMatrixService.GetProductMatrixBySpecification(new ProductMatrixSpecification()
                {
                    OfficeProductId = priceRetrival.OfficeProductId
                }).FirstOrDefault();

                if (lines == null)
                {
                    break;
                }

                decimal thisSum = lines.ProductLines.Sum(line => line.BaseCost);

                if (thisSum < minSum)
                {
                    officeProductId = priceRetrival.OfficeProductId;
                    minSum = thisSum;
                }
            }

            return new Price()
            {
                PriceExVat = minSum,
                VatPercentage =0.0m,
                OfficeProductId = officeProductId
            }; 
        }

        public Pricing Create(Pricing pricing)
        {
            DbPricing dbPricing = new DbPricing()
            {
                DateCreated= pricing.DateCreated,
                DateModified = pricing.DateModified,
                Id = pricing.Id,
                OfficeProductId = pricing.OfficeProductId
            };

            repository.DbPricing.Add(dbPricing);
            repository.SaveChanges();

            return null;
        }
    }

    /// <summary>
    /// Class used for merging price tabels
    /// </summary>
    public class PriceRetrival
    {
        public int PricingId { get; set; }
        public int OfficeProductId { get; set; }
        public int OfficeDetailId { get; set; }
        public LetterColor LetterColor { get; set; }
        public LetterPaperWeight LetterPaperWeight { get; set; }
        public LetterProcessing LetterProcessing { get; set; }
        public LetterSize LetterSize { get; set; }
        public LetterType LetterType { get; set; }
        public ProductScope ProductScope { get; set; }
        public int CountryId { get; set; }
        public int ZipId { get; set; }
        public int ContinentId { get; set; }
        public int OfficeId { get; set; }

    }
}
