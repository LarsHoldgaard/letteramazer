using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.ProductMatrix;

namespace LetterAmazer.Business.Services.Domain.Pricing
{
    public interface IPriceService
    {
        Price GetPriceByOrder(Order order);
        Price GetPriceByLetter(Letter letter);
        Price GetPriceByAddress(AddressInfo addressInfo, int pageCount);
        Price GetPriceBySpecification(PriceSpecification specification);

        Price GetPriceByMatrixLines(IEnumerable<ProductMatrixLine> matrix, int pageCount);


        Price GetPricesFromFiles(string[] filePaths, int customerId, int countryId, int originCountryId = 0);
    }
}
