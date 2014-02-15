using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class ProductMatrixFactory : IProductMatrixFactory
    {

        public ProductMatrix Create(DbProductMatrix productMatrix)
        {
            return new ProductMatrix()
            {
                ProductLines = createProductMatrixLine(productMatrix.DbProductMatrixLines),
                PriceType = (ProductMatrixPriceType)productMatrix.PriceType,
                SpanLower = productMatrix.Span_lower,
                SpanUpper = productMatrix.Span_upper
            };
        }

        public List<ProductMatrix> Create(IEnumerable<DbProductMatrix> productMatrix)
        {
            return productMatrix.Select(Create).ToList();
        }

        private ProductMatrixLine createProductMatrixLine(DbProductMatrixLines productMatrixLine)
        {
            return new ProductMatrixLine()
            {
                BaseCost = productMatrixLine.BaseCost,
                Id = productMatrixLine.Id,
                LineType = (ProductMatrixLineType)productMatrixLine.LineType,
                Title = productMatrixLine.Title
            };
        }

        private List<ProductMatrixLine> createProductMatrixLine(IEnumerable<DbProductMatrixLines> productMatrixLine)
        {
            return productMatrixLine.Select(createProductMatrixLine).ToList();
        }
    }
}
