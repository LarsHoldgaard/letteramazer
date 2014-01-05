using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.BackgroundService
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            if (args.Length == 1 && String.Compare("DEBUG", args[0], true) == 0)
            {
                Console.WriteLine("Starting Background Service in DEBUG mode...");
                new BackgroundService().Start();
                return;
            }

            // To run more than one service you have to add them here
            ServiceBase.Run(new ServiceBase[] { new BackgroundService() });
        }
    }
}
