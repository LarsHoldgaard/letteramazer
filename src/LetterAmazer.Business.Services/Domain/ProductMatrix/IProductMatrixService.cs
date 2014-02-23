using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.ProductMatrix
{
    public interface IProductMatrixService
    {
        ProductMatrix GetProductMatrixById(int id);
        IEnumerable<ProductMatrix> GetProductMatrixBySpecification(ProductMatrixSpecification specification);
        ProductMatrix Create(ProductMatrix productMatrix);
        ProductMatrix Update(ProductMatrix productMatrix);
        void Delete(ProductMatrix productMatrix);
    }
}
