//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LetterAmazer.Data.Repository.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class DbOrderlines
    {
        public int Id { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<int> LetterId { get; set; }
        public int ItemType { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public Nullable<int> PaymentMethodId { get; set; }
        public Nullable<int> CouponId { get; set; }
    
        public virtual DbLetters DbLetters { get; set; }
        public virtual DbOrders DbOrders { get; set; }
    }
}
