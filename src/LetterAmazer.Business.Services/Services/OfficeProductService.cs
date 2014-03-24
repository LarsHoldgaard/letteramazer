using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
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
            if (specification.ProductMatrixReferenceType != null)
            {
                dbProducts = dbProducts.Where(c => c.ReferenceType == (int)specification.ProductMatrixReferenceType.Value);
            }
            if (specification.LetterPaperWeight != null)
            {
                dbProducts = dbProducts.Where(c => c.LetterPaperWeight == (int)specification.LetterPaperWeight.Value);
            }
            if (specification.LetterColor != null)
            {
                dbProducts = dbProducts.Where(c => c.LetterColor == (int)specification.LetterColor.Value);
            }
            if (specification.LetterSize != null)
            {
                dbProducts = dbProducts.Where(c => c.LetterSize == (int)specification.LetterSize.Value);
            }
            if (specification.LetterProcessing != null)
            {
                dbProducts = dbProducts.Where(c => c.LetterProcessing == (int)specification.LetterProcessing.Value);
            }
            if (specification.LetterType != null)
            {
                dbProducts = dbProducts.Where(c => c.LetterType == (int)specification.LetterType.Value);
            }
            if (specification.ProductScope != null)
            {
                dbProducts = dbProducts.Where(c => c.ScopeType == (int)specification.ProductScope.Value);
            }
            if (specification.CountryId > 0)
            {
                dbProducts = dbProducts.Where(c => c.CountryId == specification.CountryId);
            }
            if (specification.ContinentId > 0)
            {
                dbProducts = dbProducts.Where(c => c.ContinentId == specification.ContinentId);
            }
            if (specification.ZipId > 0)
            {
                dbProducts = dbProducts.Where(c => c.ZipId == specification.ZipId);
            }
            if (specification.ShippingDays > 0)
            {
                dbProducts = dbProducts.Where(c => c.ShippingWeekdays <= specification.ShippingDays);
            }

            return
                officeProductFactory.Create(
                    dbProducts.Where(c=>c.Enabled).OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
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
                LetterType = (int)officeProduct.LetterDetails.LetterType,
                LetterColor = (int)officeProduct.LetterDetails.LetterColor,
                LetterPaperWeight = (int)officeProduct.LetterDetails.LetterPaperWeight,
                LetterSize = (int)officeProduct.LetterDetails.LetterSize,
                LetterProcessing = (int)officeProduct.LetterDetails.LetterProcessing,
                ReferenceType = (int)officeProduct.ReferenceType,
                OfficeProductReferenceId = officeProduct.OfficeProductReferenceId,
                ShippingWeekdays = officeProduct.ShippingDays,
                Enabled = officeProduct.Enabled,
                Automatic =officeProduct.Automatic
            };

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

            dbOfficeProduct.LetterColor = (int)officeProduct.LetterDetails.LetterColor;
            dbOfficeProduct.LetterPaperWeight = (int)officeProduct.LetterDetails.LetterPaperWeight;
            dbOfficeProduct.LetterProcessing = (int)officeProduct.LetterDetails.LetterProcessing;
            dbOfficeProduct.LetterSize = (int)officeProduct.LetterDetails.LetterSize;
            dbOfficeProduct.LetterType = (int)officeProduct.LetterDetails.LetterType;
            dbOfficeProduct.ReferenceType = (int) officeProduct.ReferenceType;
            dbOfficeProduct.OfficeProductReferenceId = officeProduct.OfficeProductReferenceId;
            dbOfficeProduct.ShippingWeekdays = officeProduct.ShippingDays;
            dbOfficeProduct.Enabled = officeProduct.Enabled;
            dbOfficeProduct.Automatic = officeProduct.Automatic;


            return GetOfficeProductById(officeProduct.Id);
        }

        public void Delete(OfficeProduct officeProduct)
        {
            var dbProduct = repository.DbOfficeProducts.FirstOrDefault(c => c.Id == officeProduct.Id);

            if (dbProduct == null)
            {
                throw new BusinessException("Product or product details is null");
            }

            repository.DbOfficeProducts.Remove(dbProduct);

            repository.SaveChanges();

        }

        /// <summary>
        /// Takes a list of office products, and group them into unique product groups from different providers
        /// </summary>
        /// <param name="officeProduct"></param>
        /// <returns></returns>
        public Dictionary<int, List<OfficeProduct>> GroupByUnique(List<OfficeProduct> officeProduct)
        {
            var list = new Dictionary<int, List<OfficeProduct>>();

            foreach (var product in officeProduct)
            {
                var listId = DoesListContainOfficeProduct(list, product);

                if (listId > 0)
                {
                    list[listId].Add(product);
                }
                else
                {
                    list.Add(list.Count + 1, new List<OfficeProduct>() { product});
                }
            }
            return list;
        }

        private int DoesListContainOfficeProduct(Dictionary<int, List<OfficeProduct>> dic, OfficeProduct officeProduct)
        {
            foreach (KeyValuePair<int, List<OfficeProduct>> keyValuePair in dic)
            {
                foreach (var product in keyValuePair.Value)
                {
                    if (product.ContinentId == officeProduct.ContinentId &&
                        product.CountryId == officeProduct.CountryId &&
                        product.ZipId == officeProduct.ZipId &&
                        product.ProductScope == officeProduct.ProductScope &&
                        product.LetterDetails.LetterColor == officeProduct.LetterDetails.LetterColor &&
                        product.LetterDetails.LetterProcessing == officeProduct.LetterDetails.LetterProcessing &&
                        product.LetterDetails.LetterPaperWeight == officeProduct.LetterDetails.LetterPaperWeight &&
                        product.LetterDetails.LetterSize == officeProduct.LetterDetails.LetterSize &&
                        product.LetterDetails.LetterType == officeProduct.LetterDetails.LetterType)
                    {
                        return keyValuePair.Key;
                    }
                }
            }
            return 0;
        }
    }
}
