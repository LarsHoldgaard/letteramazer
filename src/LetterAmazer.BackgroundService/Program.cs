using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.BackgroundService.Jobs;
using log4net;

namespace LetterAmazer.BackgroundService
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));


        static void Main(string[] args)
        {
            logger.Info("Backgroundservice started");

            log4net.Config.XmlConfigurator.Configure();
            new BackgroundService().Start(args);

            logger.Info("Backgroundservice stopped");
        }
    }
}
