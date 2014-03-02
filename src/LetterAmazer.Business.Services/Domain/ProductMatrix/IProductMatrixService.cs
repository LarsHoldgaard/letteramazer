using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.ProductMatrix
{
    public interface IProductMatrixService
    {
        ProductMatrixLine GetProductMatrixById(int id);
        IEnumerable<ProductMatrixLine> GetProductMatrixBySpecification(ProductMatrixLineSpecification specification);
        ProductMatrixLine Create(ProductMatrixLine productMatrix);
        ProductMatrixLine Update(ProductMatrixLine productMatrix);
        void Delete(ProductMatrixLine productMatrix);
    }
}
