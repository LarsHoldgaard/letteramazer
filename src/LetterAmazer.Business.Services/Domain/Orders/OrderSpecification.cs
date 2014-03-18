using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Common;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public class OrderSpecification:Specifications
    {
        public List<OrderStatus> OrderStatus { get; set; }
        public int UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public OrderSpecification()
        {
            this.OrderStatus = new List<OrderStatus>();
        }
    }
}
