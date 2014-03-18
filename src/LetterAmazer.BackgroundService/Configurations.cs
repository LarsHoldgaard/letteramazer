using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.BackgroundService
{
    public static class Configurations
    {
        public static int DeliveryLetterInterval
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["DeliveryLetterInterval"]); }
        }
    }
}
