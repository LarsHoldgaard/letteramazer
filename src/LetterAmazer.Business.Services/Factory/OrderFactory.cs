using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OrderFactory : IOrderFactory
    {
        private ILetterService letterService;
        private ICustomerService customerService;
        private IOrderService orderService;


        public OrderFactory(ILetterService letterService, ICustomerService customerService, IOrderService orderService)
        {
            this.letterService = letterService;
            this.customerService = customerService;
            this.orderService = orderService;
        }

        public Order Create(DbOrders dborder)
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
                Discount = dborder.Discount,
                OrderCode = dborder.OrderCode,
                TransactionCode = dborder.TransactionCode,
                Guid = new Guid(dborder.Guid)
            };

            var orderLines =
                orderService.GetOrderLinesBySpecification(new OrderLineSpecification() {OrderId = dborder.Id});

            order.OrderLines = orderLines;
            return order;
        }

        public List<Order> Create(List<DbOrders> orders)
        {
            return orders.Select(Create).ToList();
        }


        public List<OrderLine> Create(List<DbOrderItems> orderItemses)
        {
            return orderItemses.Select(Create).ToList();
        }
        public OrderLine Create(DbOrderItems dborderlines)
        {
            var line = new OrderLine()
            {
                Quantity = dborderlines.Quantity,
                ProductType = (ProductType)dborderlines.ItemType
            };

            if (line.ProductType == ProductType.Order && dborderlines.LetterId.HasValue)
            {
                line.BaseProduct = letterService.GetLetterById(dborderlines.LetterId.Value);
            }
            else if (line.ProductType == ProductType.Credits)
            {

            }

            return line;
        }

    }
}
