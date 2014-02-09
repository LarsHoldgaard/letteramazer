using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IOrderFactory
    {
        Order Create(DbOrders dborder);
        List<Order> Create(List<DbOrders> orders);

        List<OrderLine> Create(List<DbOrderItems> orderItemses);
        OrderLine Create(DbOrderItems dborderlines);
    }
}