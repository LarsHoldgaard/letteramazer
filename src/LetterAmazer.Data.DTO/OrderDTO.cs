using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Data.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string OrderCode { get; set; }
        public string OrderStatus { get; set; }
        public CustomerDTO Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DatePaid { get; set; }
        public Price Price { get; set; }
        public DateTime? DateSent { get; set; }

        public string TransactionCode { get; set; }


        public List<PartnerTransactionDTO> PartnerTransactions { get; set; }

       public List<OrderLineDTO> OrderLines { get; set; }
    }
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
        public string CurrencyCode { get; set; }

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
    //public class OrderCoverterDTO
    //{
    //    public int Id { get; set; }
    //    public Guid Guid { get; set; }
    //    public string OrderCode { get; set; }
    //    public string OrderStatus { get; set; }
    //    public CustomerDTO Customer { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public DateTime? DateModified { get; set; }
    //    public DateTime? DatePaid { get; set; }

    //    public DateTime? DateSent { get; set; }

    //    public string TransactionCode { get; set; }


    //    public List<PartnerTransactionDTO> PartnerTransactions { get; set; }

    //    public List<OrderLineDTO> OrderLines { get; set; }
    //}
}
