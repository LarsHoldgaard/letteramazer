﻿using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Data;
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
            IOrderService orderService;
            IFulfillmentService fulfillmentService;
            try
            {
                orderService = ServiceFactory.Get<IOrderService>();
                fulfillmentService = ServiceFactory.Get<IFulfillmentService>();

                PaginatedCriteria criteria = new PaginatedCriteria();
                criteria.PageIndex = 0;
                criteria.PageSize = 50;
                PaginatedResult<Order> orders = orderService.GetOrdersShouldBeDelivered(criteria);
                while (orders.Results.Count > 0)
                {
                    foreach (var order in orders.Results)
                    {
                        foreach (var orderItem in order.OrderItems)
                        {
                            if (orderItem.Letter.LetterStatus != LetterStatus.Created) continue;

                            try
                            {
                                fulfillmentService.DeliveryLetter(orderItem.Letter);
                                orderService.MarkLetterIsSent(orderItem.Letter.Id);
                            }
                            catch (Exception e)
                            {
                                logger.Error(e);
                            }
                        }
                    }

                    foreach (var order in orders.Results)
                    {
                        var isDone = true;
                        foreach (var orderItem in order.OrderItems)
                        {
                            if (orderItem.Letter.LetterStatus != LetterStatus.Sent)
                            {
                                isDone = false;
                                break;
                            }
                        }
                        if (isDone)
                        {
                            orderService.MarkOrderIsDone(order.Id);
                        }
                    }
                    criteria.PageIndex++;
                    orders = orderService.GetOrdersShouldBeDelivered(criteria);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}