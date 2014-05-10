using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.PriceUpdater;
using log4net;
using Quartz;

namespace LetterAmazer.BackgroundService.Jobs
{
    public class PriceUpdaterJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PriceUpdaterJob));

        public void ExecuteJob()
        {
            logger.DebugFormat("start delivery letter job at: {0}", DateTime.Now);

            IPriceUpdater priceUpdater;
            try
            {
                priceUpdater = ServiceFactory.Get<IPriceUpdater>();
                priceUpdater.Execute();
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
