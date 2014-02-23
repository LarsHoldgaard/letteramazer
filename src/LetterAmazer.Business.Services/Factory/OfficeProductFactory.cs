using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
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
                    LetterColor = (LetterColor)dbproducts.DbOfficeProductDetails.LetterColor,
                    LetterPaperWeight = (LetterPaperWeight)dbproducts.DbOfficeProductDetails.LetterPaperWeight,
                    LetterProcessing = (LetterProcessing)dbproducts.DbOfficeProductDetails.LetterProcessing,
                    LetterSize = (LetterSize)dbproducts.DbOfficeProductDetails.LetterSize,
                    LetterType = (LetterType)dbproducts.DbOfficeProductDetails.LetterType,
                },
                Id = dbproducts.Id,
                ContinentId = dbproducts.ContinentId.HasValue ? dbproducts.ContinentId.Value : 0,
                CountryId = dbproducts.CountryId.HasValue ? dbproducts.CountryId.Value : 0,
                ProductScope = (ProductScope)dbproducts.ScopeType,
                ZipId = dbproducts.ZipId.HasValue ? dbproducts.ZipId.Value : 0,
                OfficeId = dbproducts.OfficeId
            };
            officeProduct.ProductMatrices = productMatrixService.GetProductMatrixBySpecification(
                new ProductMatrixSpecification()
                {
                    OfficeProductId = dbproducts.Id,
                    ProductMatrixReferenceType = ProductMatrixReferenceType.Contractor
                });

            return officeProduct;
        }

        public List<OfficeProduct> Create(IEnumerable<DbOfficeProducts> products)
        {
            return products.Select(Create).ToList();
        }

    }
}
