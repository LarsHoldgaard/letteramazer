using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OrderLines;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using log4net;
using Quartz;
using System;

namespace LetterAmazer.BackgroundService.Jobs
{
    public class DeliveryLetterJob : AbstractInterruptableJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DeliveryLetterJob));

        protected override void ExecuteJob(IJobExecutionContext context)
        {
            logger.DebugFormat("start delivery letter job at: {0}", DateTime.Now);

            IOrderService orderService;
            IOrderLineService orderLineService;

            IFulfillmentService fulfillmentService;
            try
            {
                orderService = ServiceFactory.Get<IOrderService>();
                orderLineService = ServiceFactory.Get<IOrderLineService>();

                fulfillmentService = ServiceFactory.Get<IFulfillmentService>();

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
                    logger.Error(ex);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                logger.DebugFormat("end delivery letter job at: {0}", DateTime.Now);
            }
        }
    }
}
