using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Coupons
{
    public class CouponSpecification:Specifications
    {
        public string Code { get; set; }
        public int Id { get; set; }
    }
}
