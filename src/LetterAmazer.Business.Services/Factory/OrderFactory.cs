using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OrderFactory : IOrderFactory
    {
        private ICustomerService customerService;
        private ILetterService letterService;
        
        public OrderFactory(ICustomerService customerService, ILetterService letterService)
        {
            this.customerService = customerService;
            this.letterService = letterService;
        }

        public Order Create(DbOrders dborder, List<DbOrderlines> dborderLines)
        {
            var order = new Order()
            {
                DateCreated = dborder.DateCreated,
                DateModified = dborder.DateUpdated,
                Id = dborder.Id,
                OrderStatus = (OrderStatus)(dborder.OrderStatus),
                Customer = dborder.CustomerId.HasValue ? customerService.GetCustomerById(dborder.CustomerId.Value) : null,
                DatePaid = dborder.DatePaid.HasValue ? dborder.DatePaid.Value : DateTime.MinValue,
                VatPercentage = 0.0m,
                Cost = dborder.Cost,
                OrderCode = dborder.OrderCode,
                TransactionCode = dborder.TransactionCode,
                Guid = dborder.Guid,
                OrderLines = Create(dborderLines),
                DateSent = dborder.DateSent
            };

            return order;
        }

        public List<Order> Create(List<DbOrders> orders, List<List<DbOrderlines>> dborderLines)
        {
            if (orders.Count != dborderLines.Count)
            {
                throw new ArgumentException("Every order needs a list of orderlines");
            }

            return orders.Select((t, i) => Create(t, dborderLines[i])).ToList();
        }


        public List<OrderLine> Create(List<DbOrderlines> orderItemses)
        {
            return orderItemses.Select(Create).ToList();
        }
        public OrderLine Create(DbOrderlines dborderlines)
        {
            var line = new OrderLine()
            {
                Quantity = dborderlines.Quantity,
                ProductType = (ProductType)dborderlines.ItemType,
                Cost = dborderlines.Cost
            };

            if (line.ProductType == ProductType.Order && dborderlines.LetterId.HasValue)
            {
                line.BaseProduct = letterService.GetLetterById(dborderlines.LetterId.Value);
            }
            if (line.ProductType == ProductType.Payment && dborderlines.PaymentMethodId.HasValue &&
                dborderlines.PaymentMethodId.Value > 0)
            {
                line.PaymentMethodId = dborderlines.PaymentMethodId.HasValue ? dborderlines.PaymentMethodId.Value : 0; //paymentService.GetPaymentMethodById(dborderlines.PaymentMethodId.Value);
                line.CouponId = dborderlines.CouponId.HasValue ? dborderlines.CouponId.Value : 0;
            }
            return line;
        }

    }
}
