using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;

namespace LetterAmazer.Business.Services.Services
{
    public class DeliveryJobService : IDeliveryJobService
    {
        private IOrderService orderService;
        private IFulfillmentService fulfillmentService;

        public DeliveryJobService(IOrderService orderService, 
            IFulfillmentService fulfillmentService)
        {
            this.orderService = orderService;
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


            List<Letter> letters = new List<Letter>();
            foreach (var relevantOrder in relevantOrders)
            {
                foreach (var letter in relevantOrder.OrderLines)
                {
                    if (letter.ProductType == ProductType.Order)
                    {
                        var baseProduct = (Letter)letter.BaseProduct;
                        if (baseProduct.LetterStatus == LetterStatus.Created)
                        {
                            letters.Add(baseProduct);
                        }

                    }
                }
            }

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
