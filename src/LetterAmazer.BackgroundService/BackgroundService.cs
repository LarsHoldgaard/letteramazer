using System.Reflection;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using LetterAmazer.BackgroundService.Jobs;
using LetterAmazer.Business.Services;
using LetterAmazer.Data.Repository.Data;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Component = System.ComponentModel.Component;

namespace LetterAmazer.BackgroundService
{
    partial class BackgroundService : ServiceBase, IContainerAccessor
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BackgroundService));
        
        public BackgroundService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start this service.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            Start(args);
        }

        public void Start(string[] args)
        {
            logger.Info("Starting Background Service...");

            Container.Install(Castle.Windsor.Installer.Configuration.FromXmlFile("components.config"));

            Container.Register(Castle.MicroKernel.Registration.Component.For<LetterAmazerEntities>());

            if (args.Length == 0)
            {
                logger.Info("Running scheduled delivery");
                var delivery = new DeliveryLetterJob();
                delivery.ExecuteJob(true);
            }
            else if (args.Contains("force-schedule"))
            {
                logger.Info("Running forced delivery");
                var delivery = new DeliveryLetterJob();
                delivery.ExecuteJob(false);

            }
            else if (args.Contains("getprices"))
            {
                logger.Info("Running price update");
                var price = new PriceUpdaterJob();
                price.ExecuteJob();
            }
            logger.Info("DONE!");
        }

        
        public new IWindsorContainer Container
        {
            get { return ServiceFactory.Container; }

        }
    }
}
