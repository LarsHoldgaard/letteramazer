using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.Common;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Services.Domain.Pricing
{
    public class PriceSpecification:Specifications
    {
        public int CountryId { get; set; }
        public int ContinentId { get; set; }
        public int ZipId { get; set; }
        public int OfficeId { get; set; }

        public int OfficeProductId { get; set; }
        
        public LetterColor? LetterColor { get; set; }
        public LetterPaperWeight? LetterPaperWeight { get; set; }
        public LetterProcessing? LetterProcessing { get; set; }
        public LetterSize? LetterSize { get; set; }
        public LetterType? LetterType { get; set; }
        public int PageCount { get; set; }
        public int DeliveryDays { get; set; }

        public int ShippingDays { get; set; }

        public int OrganisationId { get; set; }

        public PriceSpecification()
        {
            PageCount = 1;
        }
    }
}
