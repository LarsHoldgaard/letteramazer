using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using Org.BouncyCastle.Asn1.Crmf;

namespace LetterAmazer.Business.Services.Domain.Letters
{
    public class LetterDetails
    {
        public int Id { get; set; }
        public LetterColor LetterColor { get; set; }
        public LetterPaperWeight LetterPaperWeight { get; set; }
        public LetterProcessing LetterProcessing { get; set; }
        public LetterSize LetterSize { get; set; }
        public LetterType LetterType { get; set; }
    }
}
