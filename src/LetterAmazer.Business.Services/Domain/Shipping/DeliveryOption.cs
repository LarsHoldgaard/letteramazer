using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;

namespace LetterAmazer.Business.Services.Domain.Shipping
{
    public class DeliveryOption
    {
        public LetterQuatity LetterQuatity { get; set; }
        
        public LetterSize PrintSize { get; set; }
    }
}
