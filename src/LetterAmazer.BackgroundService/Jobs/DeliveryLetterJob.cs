using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using log4net;
using Quartz;
using System;

namespace LetterAmazer.BackgroundService.Jobs
{
    public class DeliveryLetterJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DeliveryLetterJob));

        public void ExecuteJob(bool runSchedule)
        {
            logger.DebugFormat("start delivery letter job at: {0}", DateTime.Now);

            IDeliveryJobService deliveryJobService;
            try
            {
                deliveryJobService = ServiceFactory.Get<IDeliveryJobService>();
                deliveryJobService.Execute(runSchedule);
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
