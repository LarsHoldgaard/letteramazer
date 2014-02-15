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
            throw new NotImplementedException();
        }

        public ProductMatrix Create(ProductMatrix productMatrix)
        {
            var dbMatrix = new DbProductMatrix()
            {
                Span_lower = productMatrix.SpanLower,
                Span_upper = productMatrix.SpanUpper,
                PriceType = (int)productMatrix.PriceType,
                ReferenceType = (int)ProductMatrixReferenceType.Contractor,
                ValueId = productMatrix.ValueId,
            };
            repository.DbProductMatrix.Add(dbMatrix);
            repository.SaveChanges();

            foreach (var productMatrixLine in productMatrix.ProductLines)
            {
                dbMatrix.DbProductMatrixLines.Add(new DbProductMatrixLines()
                {
                    BaseCost = productMatrixLine.BaseCost,
                    Title = productMatrixLine.Title,
                    LineType = (int)productMatrixLine.LineType,
                    ProductMatrixId = dbMatrix.Id
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
