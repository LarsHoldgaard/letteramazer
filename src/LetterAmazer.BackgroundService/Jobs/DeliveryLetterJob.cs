using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Interfaces;
using LetterAmazer.Business.Services.Model;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                PaginatedCriteria criteria = new PaginatedCriteria();
                criteria.PageIndex = 0;
                criteria.PageSize = int.MaxValue;
                PaginatedResult<Order> orders = orderService.GetOrdersShouldBeDelivered(criteria);
                if (orders.Results.Count == 0) return;

                try
                {
                    fulfillmentService.Process(orders.Results);
                    orderService.MarkOrdersIsDone(orders.Results);
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
