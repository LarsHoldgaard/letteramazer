using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Checkout
{
    public class CheckoutLine
    {
        public int OfficeProductId { get; set; }
        public ProductType ProductType { get; set; }        
        public int Quantity { get; set; }
        public Letter Letter { get; set; }
    }
}
