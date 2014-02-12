using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OrderFactory : IOrderFactory
    {
        private ICustomerService customerService;
        
        public OrderFactory(ICustomerService customerService)
        {
            this.customerService = customerService;
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
                Guid = dborder.Guid
            };

            return order;
        }

        public List<Order> Create(List<DbOrders> orders)
        {
            return orders.Select(Create).ToList();
        }



    }
}
