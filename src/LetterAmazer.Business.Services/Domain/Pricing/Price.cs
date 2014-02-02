using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;

namespace LetterAmazer.Business.Services.Domain.Pricing
{
    public class Price
    {
        public decimal PriceExVat { get; set; }
        public decimal VatPercentage { get; set; }

    }
}
