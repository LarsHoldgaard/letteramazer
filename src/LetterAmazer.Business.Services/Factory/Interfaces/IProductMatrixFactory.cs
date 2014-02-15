using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.ProductMatrix;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IProductMatrixFactory
    {
        ProductMatrix Create(DbProductMatrix productMatrix);
        List<ProductMatrix> Create(IEnumerable<DbProductMatrix> productMatrix);
    }
}