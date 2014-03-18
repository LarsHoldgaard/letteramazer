using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IProductMatrixFactory
    {
        ProductMatrixLine Create(DbProductMatrixLines productMatrix);
        List<ProductMatrixLine> Create(IEnumerable<DbProductMatrixLines> productMatrix);
    }
}