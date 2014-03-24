using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OfficeProductFactory : IOfficeProductFactory
    {
        private IProductMatrixService productMatrixService;


        public OfficeProductFactory(IProductMatrixService productMatrixService)
        {
            this.productMatrixService = productMatrixService;
        }

        public OfficeProduct Create(DbOfficeProducts dbproducts)
        {
            
            var officeProduct = new OfficeProduct()
            {
                LetterDetails = new LetterDetails()
                {
                    LetterColor = (LetterColor)dbproducts.LetterColor,
                    LetterPaperWeight = (LetterPaperWeight)dbproducts.LetterPaperWeight,
                    LetterProcessing = (LetterProcessing)dbproducts.LetterProcessing,
                    LetterSize = (LetterSize)dbproducts.LetterSize,
                    LetterType = (LetterType)dbproducts.LetterType,
                },
                Id = dbproducts.Id,
                ContinentId = dbproducts.ContinentId.HasValue ? dbproducts.ContinentId.Value : 0,
                CountryId = dbproducts.CountryId.HasValue ? dbproducts.CountryId.Value : 0,
                ProductScope = (ProductScope)dbproducts.ScopeType,
                ZipId = dbproducts.ZipId.HasValue ? dbproducts.ZipId.Value : 0,
                OfficeId = dbproducts.OfficeId,
                ReferenceType = (ProductMatrixReferenceType)dbproducts.ReferenceType,
                OfficeProductReferenceId = dbproducts.OfficeProductReferenceId.HasValue ? dbproducts.OfficeProductReferenceId.Value : 0,
                ShippingDays = dbproducts.ShippingWeekdays,
                Enabled =dbproducts.Enabled,
                Automatic = dbproducts.Automatic
            };

            officeProduct.ProductMatrixLines = productMatrixService.GetProductMatrixBySpecification(
                new ProductMatrixLineSpecification()
                {
                    OfficeProductId = dbproducts.Id
                }).ToList();

            return officeProduct;
        }

        public List<OfficeProduct> Create(IEnumerable<DbOfficeProducts> products)
        {
            return products.Select(Create).ToList();
        }

    }
}
