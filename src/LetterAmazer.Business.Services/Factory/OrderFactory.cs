using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class OrderFactory
    {
        public Order Create(DbOrders dborder)
        {
            var order = new Order()
            {
                DateCreated = dborder.DateCreated,
                DateModified = dborder.DateUpdated,
                Id = dborder.Id,
                OrderStatus = (OrderStatus) (dborder.OrderStatus)
                //Guid = dborder.Guid,

            };
            return order;
        }
    }
}
