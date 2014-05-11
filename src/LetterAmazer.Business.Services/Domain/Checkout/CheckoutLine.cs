using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Letters;

namespace LetterAmazer.Business.Services.Domain.Checkout
{
    public class CheckoutLine
    {
        public int OfficeProductId { get; set; }
        public Letter Letter { get; set; }
    }
}
