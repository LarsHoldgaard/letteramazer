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
    
    public partial class Coupon
    {
        public int Id { get; set; }
        public decimal CouponValue { get; set; }
        public System.DateTime DateGiven { get; set; }
        public Nullable<int> EarlierCouponRef { get; set; }
        public string RefSource { get; set; }
        public string RefUserValue { get; set; }
        public System.DateTime CouponExpire { get; set; }
        public string Code { get; set; }
        public decimal CouponValueLeft { get; set; }
        public CouponStatus CouponStatus { get; set; }
    }
}
