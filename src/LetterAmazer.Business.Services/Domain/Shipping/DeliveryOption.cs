using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Data;

namespace LetterAmazer.Business.Services.Domain.Shipping
{
    public class DeliveryOption
    {
        public LetterQuatity LetterQuatity { get; set; }
        public PrintQuality PrintQuality { get; set; }
        public PrintSize PrintSize { get; set; }
    }
}
