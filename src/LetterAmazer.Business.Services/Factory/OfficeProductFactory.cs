using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OfficeProductFactory : IOfficeProductFactory
    {
        public OfficeProductFactory()
        {
            
        }

        public OfficeProduct Create(DbOfficeProducts dbproducts)
        {
            
            return new OfficeProduct()
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
        }

        public List<OfficeProduct> Create(List<DbOfficeProducts> products)
        {
            return products.Select(Create).ToList();
        }



    }
}
