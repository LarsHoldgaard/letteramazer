using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IOrderFactory
    {
        Order Create(DbOrders dborder, List<DbOrderlines> dborderLines);
        List<Order> Create(List<DbOrders> orders, List<List<DbOrderlines>> dborderLines);
        List<OrderLine> Create(List<DbOrderlines> orderItemses);
        OrderLine Create(DbOrderlines dborderlines);

    }
}