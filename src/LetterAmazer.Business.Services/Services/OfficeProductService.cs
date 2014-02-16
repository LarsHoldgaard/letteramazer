using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class OfficeProductService : IOfficeProductService
    {
        private LetterAmazerEntities repository;
        
        private IOfficeProductFactory officeProductFactory;

        public OfficeProductService(LetterAmazerEntities repository,IOfficeProductFactory officeProductFactory)
        {
            this.repository = repository;
            this.officeProductFactory = officeProductFactory;
        }

        public OfficeProduct GetOfficeProductById(int id)
        {
            var dbOfficeProduct = repository.DbOfficeProducts.FirstOrDefault(c => c.Id == id);

            if (dbOfficeProduct == null)
            {
                return null;
            }

            var officeProduct = officeProductFactory.Create(dbOfficeProduct);
            return officeProduct;
        }

        public List<OfficeProduct> GetOfficeProductBySpecification(OfficeProductSpecification specification)
        {
            IQueryable<DbOfficeProducts> dbProducts = repository.DbOfficeProducts;

            if (specification.Id > 0)
            {
                dbProducts = dbProducts.Where(c => c.Id == specification.Id);
            }

            return
                officeProductFactory.Create(
                    dbProducts.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public OfficeProduct Create(OfficeProduct officeProduct)
        {
            DbOfficeProducts dbOfficeProduct = new DbOfficeProducts()
            {
                OfficeId = officeProduct.OfficeId,
                ContinentId = officeProduct.ContinentId,
                ScopeType = (int)officeProduct.ProductScope,
                CountryId = officeProduct.CountryId,
                ZipId = officeProduct.ZipId,
                
            };

            DbOfficeProductDetails dbDetails = new DbOfficeProductDetails()
            {
                LetterType = (int)officeProduct.LetterDetails.LetterType,
                LetterColor = (int)officeProduct.LetterDetails.LetterColor,
                LetterPaperWeight = (int)officeProduct.LetterDetails.LetterPaperWeight,
                LetterSize = (int)officeProduct.LetterDetails.LetterSize,
                LetterProcessing = (int)officeProduct.LetterDetails.LetterProcessing,
            };
            dbOfficeProduct.DbOfficeProductDetails = dbDetails;

            repository.DbOfficeProducts.Add(dbOfficeProduct);
            repository.SaveChanges();

     

            return GetOfficeProductById(dbOfficeProduct.Id);

        }

        public OfficeProduct Update(OfficeProduct officeProduct)
        {
            var dbOfficeProduct = repository.DbOfficeProducts.FirstOrDefault(c => c.Id == officeProduct.Id);

            if (dbOfficeProduct == null)
            {
                return null;
            }

            dbOfficeProduct.OfficeId = officeProduct.OfficeId;
            dbOfficeProduct.ZipId = officeProduct.ZipId;
            dbOfficeProduct.CountryId = officeProduct.CountryId;
            dbOfficeProduct.ContinentId = officeProduct.ContinentId;
            dbOfficeProduct.ScopeType = (int)officeProduct.ProductScope;

            dbOfficeProduct.DbOfficeProductDetails.LetterColor = (int)officeProduct.LetterDetails.LetterColor;
            dbOfficeProduct.DbOfficeProductDetails.LetterPaperWeight = (int)officeProduct.LetterDetails.LetterPaperWeight;
            dbOfficeProduct.DbOfficeProductDetails.LetterProcessing = (int)officeProduct.LetterDetails.LetterProcessing;
            dbOfficeProduct.DbOfficeProductDetails.LetterSize = (int)officeProduct.LetterDetails.LetterSize;
            dbOfficeProduct.DbOfficeProductDetails.LetterType = (int)officeProduct.LetterDetails.LetterType;


            return GetOfficeProductById(officeProduct.Id);
        }

        public void Delete(OfficeProduct officeProduct)
        {
            var dbProductDetails =
                repository.DbOfficeProductDetails.FirstOrDefault(c => c.Id == officeProduct.LetterDetails.Id);
            var dbProduct = repository.DbOfficeProducts.FirstOrDefault(c => c.Id == officeProduct.Id);

            if (dbProduct == null || dbProductDetails == null)
            {
                throw new BusinessException("Product or product details is null");
            }

            repository.DbOfficeProducts.Remove(dbProduct);
            repository.DbOfficeProductDetails.Remove(dbProductDetails);

            repository.SaveChanges();

        }
    }
}
