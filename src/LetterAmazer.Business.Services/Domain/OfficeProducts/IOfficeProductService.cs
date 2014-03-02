using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Offices;

namespace LetterAmazer.Business.Services.Domain.OfficeProducts
{
    public interface IOfficeProductService
    {
        OfficeProduct GetOfficeProductById(int letterId);
        List<OfficeProduct> GetOfficeProductBySpecification(OfficeProductSpecification specification);
        OfficeProduct Create(OfficeProduct letter);
        OfficeProduct Update(OfficeProduct letter);
        void Delete(OfficeProduct letter);

        Dictionary<int, List<OfficeProduct>> GroupByUnique(List<OfficeProduct> officeProduct);
    }
}
