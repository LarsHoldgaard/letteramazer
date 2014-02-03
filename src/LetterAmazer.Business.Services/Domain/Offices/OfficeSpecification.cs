using LetterAmazer.Business.Services.Domain.Common;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Services.Domain.Offices
{
    public class OfficeSpecification:Specifications
    {
        public int CountryId { get; set; }
        public LetterSize  LetterSize { get; set; }
        
    }
}
