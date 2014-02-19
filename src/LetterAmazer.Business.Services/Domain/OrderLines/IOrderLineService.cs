using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.OrderLines
{
    public interface IOrderLineService
    {
        OrderLine Create(OrderLine order);
        OrderLine Update(OrderLine order);
        List<OrderLine> GetOrderlineBySpecification(OrderLineSpecification specification);
        OrderLine GetOrderlineById(int orderLineId);
        void Delete(OrderLine orderline);
    }
}
