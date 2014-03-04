using System;
using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string OrderCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DatePaid { get; set; }

        public DateTime? DateSent { get; set; }

        public string TransactionCode { get; set; }

        public Price Price { get; set; }

        
        public List<OrderLine> OrderLines { get; set; }
        public Order()
        {
            this.OrderLines = new List<OrderLine>();
            this.OrderStatus =OrderStatus.Created;

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public decimal CostFromLines()
        {
            var lines = OrderLines.Where(c => c.ProductType == ProductType.Letter);
            return lines.Sum(c => c.Price.PriceExVat);
        }
    }
}
