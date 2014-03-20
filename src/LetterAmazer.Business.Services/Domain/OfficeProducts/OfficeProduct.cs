using System.Collections.Generic;
using iTextSharp.text;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.ProductMatrix;

namespace LetterAmazer.Business.Services.Domain.OfficeProducts
{
    /// <summary>
    /// An office has a wide range of products to a specific area, with specific details
    /// An office might have 10.000+ offices products
    /// </summary>
    public class OfficeProduct
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public ProductScope ProductScope { get; set; }

        public int CountryId { get; set; }
        public int ContinentId { get; set; }
        public int ZipId { get; set; }
        public LetterDetails LetterDetails { get; set; }
        public List<ProductMatrix.ProductMatrixLine> ProductMatrixLines { get; set; }

        public ProductMatrixReferenceType ReferenceType { get; set; }

        public int OfficeProductReferenceId { get; set; }
        public int ShippingDays { get; set; }

        public bool Enabled { get; set; }

        public OfficeProduct()
        {
            this.ProductMatrixLines = new List<ProductMatrixLine>();
        }
    }
}
