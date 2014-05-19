using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Caching;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class ProductMatrixService:IProductMatrixService
    {
        private LetterAmazerEntities repository;

        private IProductMatrixFactory productMatrixFactory;
        private ICacheService cacheService;

        public ProductMatrixService(LetterAmazerEntities repository, IProductMatrixFactory productMatrixFactory,ICacheService cacheService)
        {
            this.repository = repository;
            this.productMatrixFactory = productMatrixFactory;
            this.cacheService = cacheService;
        }

        public ProductMatrixLine GetProductMatrixById(int id)
        {
            var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, id.ToString());
            if (!cacheService.ContainsKey(cacheKey))
            {
                var dbProductMatrix = repository.DbProductMatrixLines.FirstOrDefault(c => c.Id == id);

                if (dbProductMatrix == null)
                {
                    throw new BusinessException("Product matrix wasn't found");
                }

                var productMatrix = productMatrixFactory.Create(dbProductMatrix);
                cacheService.Create(cacheKey, productMatrix);
                return productMatrix;
            }
            return (ProductMatrixLine) (cacheService.GetById(cacheKey));
        }

        public IEnumerable<ProductMatrixLine> GetProductMatrixBySpecification(ProductMatrixLineSpecification specification)
        {
            IQueryable<DbProductMatrixLines> dbProductMatrices = repository.DbProductMatrixLines;

            if (specification.OfficeProductId > 0)
            {
                dbProductMatrices = dbProductMatrices.Where(c => c.OfficeProductId == specification.OfficeProductId);
            }
            if (specification.PageCount > 1)
            {
                dbProductMatrices = dbProductMatrices.Where(c => c.Span_lower <= specification.PageCount);
            }
            

            return productMatrixFactory.Create(dbProductMatrices.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public ProductMatrixLine Create(ProductMatrixLine productMatrixLine)
        {
            var dbMatrixLine = new DbProductMatrixLines()
            {
                BaseCost = productMatrixLine.BaseCost,
                OfficeProductId = productMatrixLine.OfficeProductId,
                Span_lower = productMatrixLine.SpanLower,
                Span_upper = productMatrixLine.SpanUpper,
                LineType = (int) productMatrixLine.LineType,
                PriceType = (int) productMatrixLine.PriceType,
                Title = productMatrixLine.Title,
                CurrencyId = (int)productMatrixLine.CurrencyCode
            };

            repository.DbProductMatrixLines.Add(dbMatrixLine);
            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetProductMatrixById",dbMatrixLine.Id.ToString()));

            return GetProductMatrixById(dbMatrixLine.Id);
        }

        public ProductMatrixLine Update(ProductMatrixLine productMatrixLine)
        {
            var dbMatrixLine = repository.DbProductMatrixLines.FirstOrDefault(c => c.Id == productMatrixLine.Id);
            dbMatrixLine.BaseCost = productMatrixLine.BaseCost;
            dbMatrixLine.OfficeProductId = productMatrixLine.OfficeProductId;
            dbMatrixLine.Span_lower = productMatrixLine.SpanLower;
            dbMatrixLine.Span_upper = productMatrixLine.SpanUpper;
            dbMatrixLine.LineType = (int)productMatrixLine.LineType;
            dbMatrixLine.PriceType = (int)productMatrixLine.PriceType;
            dbMatrixLine.Title = productMatrixLine.Title;
            dbMatrixLine.CurrencyId = (int) productMatrixLine.CurrencyCode;

            cacheService.Delete(cacheService.GetCacheKey("GetProductMatrixById", dbMatrixLine.Id.ToString()));

            return GetProductMatrixById(dbMatrixLine.Id);
        }

        public void Delete(ProductMatrixLine productMatrixLine)
        {
            var dbProductMatrix = repository.DbProductMatrixLines.FirstOrDefault(c => c.Id == productMatrixLine.Id);
            
            repository.DbProductMatrixLines.Remove(dbProductMatrix);
            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetProductMatrixById", productMatrixLine.Id.ToString()));

        }
    }
}
