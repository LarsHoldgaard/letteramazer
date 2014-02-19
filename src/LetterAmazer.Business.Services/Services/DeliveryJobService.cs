using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Services
{
    public class DeliveryJobService : IDeliveryJobService
    {
        private IOrderService orderService;
        private IOrderLineService orderLineService;
        private IFulfillmentService fulfillmentService;

        public DeliveryJobService(IOrderService orderService, 
            IOrderLineService orderLineService,
            IFulfillmentService fulfillmentService)
        {
            this.orderService = orderService;
            this.orderLineService = orderLineService;
            this.fulfillmentService = fulfillmentService;
        }

        public void Execute()
        {
            var relevantOrders = orderService.GetOrderBySpecification(new OrderSpecification()
            {
                OrderStatus = new List<OrderStatus>()
                    {
                        OrderStatus.Paid,
                        OrderStatus.InProgress
                    }
            });

            List<Letter> letters = (from relevantOrder in relevantOrders
                                    from orderLine in orderLineService.GetOrderlineBySpecification(new OrderLineSpecification()
                                    {
                                        OrderId = relevantOrder.Id
                                    })
                                    where orderLine.ProductType == ProductType.Order
                                    select (Letter)orderLine.BaseProduct
                                        into letter
                                        where letter.LetterStatus == LetterStatus.Created
                                        select letter).ToList();

            if (!letters.Any())
            {
                return;
            }

            try
            {
                fulfillmentService.Process(letters);
            }
            catch (Exception ex)
            {
             //   logger.Error(ex);
            }
        }
    }
}
