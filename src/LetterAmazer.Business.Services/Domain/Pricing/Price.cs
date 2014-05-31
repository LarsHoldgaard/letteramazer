using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.Currencies;
using LetterAmazer.Business.Services.Exceptions;

namespace LetterAmazer.Business.Services.Domain.Pricing
{
    public class Price
    {
        #region Properties

        public decimal PriceExVat { get; set; }

        /// <summary>
        /// This is the percentage of the product of VAT
        /// </summary>
        public decimal VatPercentage { get; set; }

        /// <summary>
        /// The currencycode this price is presented in
        /// </summary>
        public CurrencyCode CurrencyCode { get; set; }

        public int OfficeProductId { get; set; }

        /// <summary>
        /// This is the price which is actually VAT
        /// </summary>
        public decimal VatPrice
        {
            get
            {
                return (PriceExVat * VatPercentage);
            }
        }

        public decimal Total
        {
            get { return PriceExVat * (1 + VatPercentage); }
        }

        #endregion

        public Price()
        {

        }

        public void AddPrice(Price price)
        {
            this.PriceExVat += price.PriceExVat;
        }
    }
}
