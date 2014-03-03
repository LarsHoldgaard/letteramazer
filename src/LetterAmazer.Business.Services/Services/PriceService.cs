using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Utils;
using LetterAmazer.Data.Repository.Data;
using ProductType = LetterAmazer.Business.Services.Domain.Products.ProductType;

namespace LetterAmazer.Business.Services.Services
{
    public class PriceService : IPriceService
    {
        private ICountryService countryService;
        private LetterAmazerEntities repository;
        private IProductMatrixService productMatrixService;
        private IOfficeProductService offerProductService;
        private IOrganisationService organisationService;

        public PriceService(ICountryService countryService,
            LetterAmazerEntities repository, IProductMatrixService productMatrixService, 
            IOfficeProductService offerProductService, IOrganisationService organisationService
            )
        {
            this.countryService = countryService;
            this.repository = repository;
            this.productMatrixService = productMatrixService;
            this.offerProductService = offerProductService;
            this.organisationService = organisationService;
        }

        public Price GetPriceByOrder(Order order)
        {
            
            var price = new Price();
            foreach (var orderLine in order.OrderLines)
            {
                if (orderLine.ProductType == ProductType.Letter)
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
            return GetPriceByAddress(letter.ToAddress,letter.LetterContent.PageCount);
        }

        public Price GetPriceByAddress(AddressInfo addressInfo, int pageCount)
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
                LetterPaperWeight = LetterPaperWeight.Eight,
                PageCount = pageCount
            });

            return price;
        }

        public Price GetPriceBySpecification(PriceSpecification specification)
        {
            IQueryable<DbOfficeProducts> prices =
                repository.DbOfficeProducts.Where(c => c.ReferenceType == (int) ProductMatrixReferenceType.Sales);

            if (specification.LetterColor.HasValue)
            {
                prices = prices.Where(c => c.LetterColor == (int)specification.LetterColor.Value);
            }
            if (specification.LetterPaperWeight.HasValue)
            {
                prices = prices.Where(c => c.LetterPaperWeight == (int)specification.LetterPaperWeight.Value);
            }
            if (specification.LetterProcessing.HasValue)
            {
                prices = prices.Where(c => c.LetterProcessing == (int)specification.LetterProcessing.Value);
            }
            if (specification.LetterSize.HasValue)
            {
                prices = prices.Where(c => c.LetterSize == (int)specification.LetterSize.Value);
            }
            if (specification.LetterType.HasValue)
            {
                prices = prices.Where(c => c.LetterType == (int)specification.LetterType.Value);
            }
            
            decimal minCost = decimal.MaxValue;
            int officeProductId = 0;
            foreach (var officeProduct in prices)
            {
                var matrix = productMatrixService.GetProductMatrixBySpecification(new ProductMatrixLineSpecification()
                {
                    OfficeProductId = officeProduct.Id,
                    PageCount = specification.PageCount
                });

                var productCost = GetPriceByMatrixLines(matrix,specification.PageCount).PriceExVat;
                if (minCost > productCost)
                {
                    minCost = productCost;
                    officeProductId = officeProduct.Id;
                }
            }

            if (minCost == decimal.MaxValue)
            {
                throw new BusinessException("No price for this specification");
            }

            var addVat = isVatAdded(specification);

            return new Price()
            {
                OfficeProductId = officeProductId,
                PriceExVat =minCost,
                VatPercentage = addVat ? 0.25m : 0.0m
            };
        }

        private bool isVatAdded(PriceSpecification specification)
        {
            bool addVat = true;
            if (specification.OrganisationId > 0)
            {
                var organisation = organisationService.GetOrganisationById(specification.OrganisationId);

                if (organisation.Address.Country.InsideEu)
                {
                    if (!string.IsNullOrEmpty(organisation.Address.VatNr))
                    {
                        addVat = false;
                    }
                }
                else
                {
                    addVat = false;
                }
            }
            return addVat;
        }

        public Price GetPriceByMatrixLines(IEnumerable<ProductMatrixLine> matrix, int pageCount)
        {
            decimal productCost = 0.0m;
            for (int page = 1; page <= pageCount; page++)
            {
                var lines = matrix.Where(c => c.SpanLower <= page && c.SpanUpper >= page);

                if (!lines.Any())
                {
                    throw new BusinessException("No price for this page");
                }

                foreach (var productMatrixLine in lines)
                {
                    if (productMatrixLine.PriceType == ProductMatrixPriceType.PrPage)
                    {
                        productCost += productMatrixLine.BaseCost;
                    }
                    if (productMatrixLine.PriceType == ProductMatrixPriceType.Span &&
                        productMatrixLine.SpanLower == page)
                    {
                        productCost += productMatrixLine.BaseCost;
                    }
                }
            }
            return new Price()
            {
                PriceExVat = productCost,
                VatPercentage = 0.0m,
                OfficeProductId = 0
            }; 
        }

    }

}
