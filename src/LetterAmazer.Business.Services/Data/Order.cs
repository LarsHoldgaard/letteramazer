//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LetterAmazer.Business.Services.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.Letters = new HashSet<Letter>();
        }
    
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string Guid { get; set; }
        public string OrderCode { get; set; }
        public string TransactionCode { get; set; }
        public decimal Cost { get; set; }
        public string CouponCode { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    
        public virtual ICollection<Letter> Letters { get; set; }
        public virtual Customer Customer { get; set; }
    }
}