using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Services.Domain.Organisation
{
    public class OrganisationSettings
    {
        public int Id { get; set; }
        public LetterColor? LetterColor { get; set; }
        public LetterPaperWeight? LetterPaperWeight { get; set; }
        public LetterProcessing? LetterProcessing { get; set; }
        public LetterSize? LetterSize { get; set; }
        public LetterType? LetterType { get; set; }
        public int? PreferedCountryId { get; set; }
    }
}
