using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;

namespace LetterAmazer.Business.Services.Domain.Orders
{
    public interface IOrderService
    {
        Order Create(Order order);

        Order Update(Order order);
        void UpdateByLetters(IEnumerable<Letter> letters);

        List<Order> GetOrderBySpecification(OrderSpecification specification);
        Order GetOrderById(int orderId);
        void Delete(Order order);

        List<OrderLine> GetOrderLinesBySpecification(OrderLineSpecification specification);
    }
}
