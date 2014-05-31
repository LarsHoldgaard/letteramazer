using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using LetterAmazer.Business.Services.Domain.Common;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Payments
{
    public class PaymentMethodSpecification:Specifications
    {
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentType? PaymentType { get; set; }
    }
}
