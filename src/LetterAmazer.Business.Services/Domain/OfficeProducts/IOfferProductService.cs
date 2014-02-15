using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.OfficeProducts
{
    public interface IOfferProductService
    {
        OfficeProduct GetOfficeProductById(int letterId);
        List<OfficeProduct> GetOfficeProductBySpecification(OfficeProductSpecification specification);
        OfficeProduct Create(OfficeProduct letter);
        OfficeProduct Update(OfficeProduct letter);
        void Delete(OfficeProduct letter);
    }
}
