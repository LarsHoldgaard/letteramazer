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

        /// <summary>
        /// This is the percentage of the product of VAT
        /// </summary>
        public decimal VatPercentage { get; set; }

        
        public int OfficeProductId { get; set; }

        /// <summary>
        /// This is the price which is actually VAT
        /// </summary>
        public decimal VatPrice 
        {
            get
            {
                return (PriceExVat*VatPercentage)/(1+VatPercentage);
            } 
        }

        public decimal Total
        {
            get { return PriceExVat*(1 + VatPercentage); }
        }
    }
}
