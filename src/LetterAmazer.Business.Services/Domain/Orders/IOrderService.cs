using System.Collections.Generic;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public interface IOrderService
    {
        Order Create(Order order);
        Order Update(Order order);

        List<Order> GetOrderBySpecification(OrderSpecification specification);
        Order GetOrderById(int orderId);
        void Delete(Order order);

        List<OrderLine> GetOrderLinesBySpecification(OrderLineSpecification specification);
    }
}
