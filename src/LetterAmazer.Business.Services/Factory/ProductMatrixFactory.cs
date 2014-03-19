using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Currencies;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class ProductMatrixFactory : IProductMatrixFactory
    {


        public ProductMatrixLine Create(DbProductMatrixLines productMatrixLine)
        {
            return new ProductMatrixLine()
            {
                BaseCost = productMatrixLine.BaseCost,
                Id = productMatrixLine.Id,
                LineType = (ProductMatrixLineType)productMatrixLine.LineType,
                Title = productMatrixLine.Title,
                OfficeProductId = productMatrixLine.OfficeProductId,
                PriceType = (ProductMatrixPriceType)productMatrixLine.PriceType,
                SpanLower = productMatrixLine.Span_lower,
                SpanUpper = productMatrixLine.Span_upper,
                CurrencyCode = (CurrencyCode)productMatrixLine.CurrencyId
            };
        }

        public List<ProductMatrixLine> Create(IEnumerable<DbProductMatrixLines> productMatrixLine)
        {
            return productMatrixLine.Select(Create).ToList();
        }
    }
}
