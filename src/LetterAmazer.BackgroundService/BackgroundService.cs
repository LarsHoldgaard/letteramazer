using Common.Logging;
using LetterAmazer.BackgroundService.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LetterAmazer.BackgroundService
{
    partial class BackgroundService : ServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BackgroundService));

        public const string MyServiceName = "LetterAmazer.BackgroundService";

        private const string GROUP_NAME = "LetterAmazer";

        private const string DELIVERY_LETTER_JOB = "DeliveryLetterJob";
        private const string DELIVERY_LETTER_TRIGGER = "DeliveryLetterTrigger";

        private const int RetryScheduleJobsAtLeastXTimes = 10;
        private const int RetryScheduleJobsAfterXSeconds = 10;

        private IScheduler scheduler = null;
        private bool stopping = false;

        public BackgroundService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start this service.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            Start();
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            logger.Info("Stop Background Service...");
            stopping = true;
            if (scheduler != null)
            {
                IList<IJobExecutionContext> jobExecutionContexts = scheduler.GetCurrentlyExecutingJobs();
                foreach (var jobExecutionContext in jobExecutionContexts)
                {
                    if (jobExecutionContext.JobDetail.JobType == typeof(DeliveryLetterJob))
                    {
                        try
                        {
                            logger.InfoFormat("Stopping job: {0} at {1}", jobExecutionContext.JobDetail.Key, DateTime.Now);
                            scheduler.Interrupt(jobExecutionContext.JobDetail.Key);
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorFormat("Error on stopping job: {0}{1}{2}", jobExecutionContext.JobDetail.Key, Environment.NewLine, ex);
                            throw new JobExecutionException(ex);
                        }
                    }
                }
                scheduler.Shutdown();
            }
            logger.Info("DONE!");
        }

        public void Start()
        {
            logger.Info("Starting Background Service...");

            ISchedulerFactory scheduleFactory = new StdSchedulerFactory();
            scheduler = scheduleFactory.GetScheduler();
            scheduler.Start();

            //ScheduleDeliveryLetterJob();

            // we need to start a thread to avoid Windows stop the service if it started too long
            ThreadStart threadStart = new System.Threading.ThreadStart(ScheduleJobs);
            new Thread(threadStart).Start();

            logger.Info("DONE!");
        }

        private void ScheduleJobs()
        {
            TryScheduleJobs(RetryScheduleJobsAtLeastXTimes);
        }

        private void TryScheduleJobs(int atLeastXTimes)
        {
            try
            {
                ScheduleDeliveryLetterJob();
            }
            catch (Exception ex)
            {
                if (stopping) return;

                logger.Error(ex);
                logger.InfoFormat("Retry schedule jobs {0} times", atLeastXTimes);

                // sleep 1 minutes and retry
                System.Threading.Thread.Sleep(1000 * RetryScheduleJobsAfterXSeconds);
                atLeastXTimes -= 1;
                if (atLeastXTimes > 0)
                {
                    TryScheduleJobs(atLeastXTimes);
                }
            }
        }

        private void ScheduleDeliveryLetterJob()
        {
            IJobDetail jobDetail = new JobDetailImpl(DELIVERY_LETTER_JOB, GROUP_NAME, typeof(DeliveryLetterJob));
            ITrigger trigger = new SimpleTriggerImpl(DELIVERY_LETTER_TRIGGER, SimpleTriggerImpl.RepeatIndefinitely, TimeSpan.FromSeconds(Configurations.DeliveryLetterInterval));
            ScheduleJob(jobDetail, trigger);
        }

        private void ScheduleJob(IJobDetail jobDetail, ITrigger trigger)
        {
            if (scheduler == null) return;

            if (scheduler.GetJobDetail(jobDetail.Key) != null)
            {
                scheduler.UnscheduleJob(trigger.Key);
                scheduler.DeleteJob(jobDetail.Key);
            }

            logger.InfoFormat("Schedule job {0}", jobDetail.Key);
            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
