using System.Collections.Generic;
using LetterAmazer.Business.Services.Model;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public interface IOrderService
    {
        string CreateOrder(OrderContext orderContext);
        void MarkOrderIsPaid(int orderId);
        PaginatedResult<Order> GetOrdersShouldBeDelivered(PaginatedCriteria criteria);
        void MarkLetterIsSent(int letterId);
        void MarkOrderIsDone(int orderId);
        void MarkOrdersIsDone(IList<Order> orders);

        PaginatedResult<Order> GetOrders(OrderCriteria criteria);

        string AddFunds(int customerId, decimal price);
        void AddFundsForAccount(int orderId);

        Order GetOrderById(int orderId);
        void DeleteOrder(int orderId);
    }
}
