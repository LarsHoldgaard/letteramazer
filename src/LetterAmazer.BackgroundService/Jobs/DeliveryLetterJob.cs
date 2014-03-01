using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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

                orderService = Container.Resolve<IOrderService>();
                
                fulfillmentService = ServiceFactory.Get<IFulfillmentService>();

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
                        if (letter.ProductType == ProductType.Letter)
                        {
                            var baseProduct = (Letter) letter.BaseProduct;
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


        public new IWindsorContainer Container
        {
            get { return ServiceFactory.Container; }
        }
    }
}
