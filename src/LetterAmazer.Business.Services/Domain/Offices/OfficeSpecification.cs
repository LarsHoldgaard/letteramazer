using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Common;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Services.Domain.Offices
{
    public class OfficeSpecification:Specifications
    {
        public int CountryId { get; set; }
        public LetterSize  LetterSize { get; set; }
        public PrintQuality PrintQuality { get; set; }
        public LetterQuatity LetterQuatity { get; set; }
    }
}
