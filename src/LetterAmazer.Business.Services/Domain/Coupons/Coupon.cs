using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Coupons
{
    public class Coupon
    {
        public int Id { get; set; }
        public decimal CouponValue { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int EarlierCouponRef { get; set; }
        public string RefSource { get; set; }
        public string RefUserValue { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Code { get; set; }
        public decimal CouponValueLeft { get; set; }
        public CouponStatus CouponStatus { get; set; }
    }
}
