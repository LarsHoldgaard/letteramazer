using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ProductMatrixService(LetterAmazerEntities repository, IProductMatrixFactory productMatrixFactory)
        {
            this.repository = repository;
            this.productMatrixFactory = productMatrixFactory;
        }

        public ProductMatrix GetProductMatrixById(int id)
        {
            var dbProductMatrix = repository.DbProductMatrix.FirstOrDefault(c => c.Id == id);

            if (dbProductMatrix == null)
            {
                throw new BusinessException("Product matrix wasn't found");
            }

            var productMatrix = productMatrixFactory.Create(dbProductMatrix);
            return productMatrix;
        }

        public List<ProductMatrix> GetProductMatrixBySpecification(ProductMatrixSpecification specification)
        {
            IQueryable<DbProductMatrix> dbProductMatrices = repository.DbProductMatrix;

            if (specification.OfficeProductId > 0)
            {
                dbProductMatrices = dbProductMatrices.Where(c => c.ValueId == specification.OfficeProductId && c.ReferenceType == (int)ProductMatrixReferenceType.Contractor);
            }
            if (specification.PriceId > 0)
            {
                dbProductMatrices = dbProductMatrices.Where(c => c.ValueId == specification.PriceId && c.ReferenceType == (int)ProductMatrixReferenceType.Sales);
            }

            return productMatrixFactory.Create(dbProductMatrices.OrderBy(c => c.Id).Skip(specification.Skip).Take(specification.Take).ToList());
        }

        public ProductMatrix Create(ProductMatrix productMatrix)
        {
            var dbMatrix = new DbProductMatrix()
            {
                PriceType = (int)productMatrix.PriceType,
                ReferenceType = (int)productMatrix.ReferenceType,
                ValueId = productMatrix.ValueId,
            };

            if (productMatrix.PriceType == ProductMatrixPriceType.FirstPage)
            {
                if (dbMatrix.Span_lower == 0)
                {
                    dbMatrix.Span_lower = 1;
                }
                if (dbMatrix.Span_upper == 0)
                {
                    dbMatrix.Span_upper = 1;
                }
            }

            repository.DbProductMatrix.Add(dbMatrix);
            repository.SaveChanges();

            foreach (var productMatrixLine in productMatrix.ProductLines)
            {
                dbMatrix.DbProductMatrixLines.Add(new DbProductMatrixLines()
                {
                    BaseCost = productMatrixLine.BaseCost,
                    Title = productMatrixLine.Title,
                    LineType = (int)productMatrixLine.LineType,
                    ProductMatrixId = dbMatrix.Id,
                });
                repository.SaveChanges();
            }

            return GetProductMatrixById(dbMatrix.Id);
        }

        public ProductMatrix Update(ProductMatrix productMatrix)
        {
            throw new NotImplementedException();
        }

        public void Delete(ProductMatrix productMatrix)
        {
            var dbProductMatrix = repository.DbProductMatrix.FirstOrDefault(c => c.Id == productMatrix.Id);
            var dbProductMatrixLines = repository.DbProductMatrixLines.Where(c => c.ProductMatrixId == productMatrix.Id);

            foreach (var matrixLine in dbProductMatrixLines)
            {
                repository.DbProductMatrixLines.Remove(matrixLine);
            }
            repository.DbProductMatrix.Remove(dbProductMatrix);
            repository.SaveChanges();

        }
    }
}
