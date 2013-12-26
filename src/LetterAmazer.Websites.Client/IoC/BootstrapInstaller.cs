using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace LetterAmazer.Websites.Client.IoC
{
    public class BootstrapInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<Castle.Facilities.Startable.StartableFacility>();
        }
    }
}