using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface IOrderService
    {
        string CreateOrder(OrderContext orderContext);
        void MarkOrderIsPaid(int orderId);
        PaginatedResult<Order> GetOrdersShouldBeDelivered(PaginatedCriteria criteria);
        void MarkLetterIsSent(int letterId);
        void MarkOrderIsDone(int orderId);
        void MarkOrdersIsDone(IList<Order> orders);
    }
}
