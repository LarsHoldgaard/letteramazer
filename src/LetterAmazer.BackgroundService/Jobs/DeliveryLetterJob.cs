using System.Collections.Generic;
using System.Linq;
using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
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
            IFulfillmentService fulfillmentService;
            try
            {
                orderService = ServiceFactory.Get<IOrderService>();
                
                fulfillmentService = ServiceFactory.Get<IFulfillmentService>();

                var relevantOrders = orderService.GetOrderBySpecification(new OrderSpecification()
                {
                    OrderStatus = new List<OrderStatus>()
                    {
                        OrderStatus.Paid,
                        OrderStatus.InProgress
                    }
                });

                var allLetters = relevantOrders.
                    Select(c => c.OrderLines.Where(d => d.ProductType == ProductType.Order));
                
                allLetters =
                    allLetters.Select(c => c.Where(d => ((Letter) d.BaseProduct).LetterStatus == LetterStatus.Created))
                        .ToList();

                if (!allLetters.Any())
                {
                    return;
                }

               var letters = new List<Letter>();
                foreach (var item1 in allLetters)
                {
                    letters.AddRange(item1.Select(orderLine => (Letter) orderLine.BaseProduct));
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
