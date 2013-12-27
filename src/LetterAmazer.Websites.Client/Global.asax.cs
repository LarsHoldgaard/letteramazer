using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using LetterAmazer.Business.Services;
using LetterAmazer.Websites.Client.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LetterAmazer.Websites.Client
{
    public class MvcApplication : System.Web.HttpApplication, IContainerAccessor
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            InitializeContainer();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void InitializeContainer()
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);

            Container.Register(Component.For<IWindsorContainer>().Instance(this.Container));
            Container.Install(new BootstrapInstaller());
            Container.Install(Configuration.FromXmlFile("components.config"));
            Container.Install(new WebWindsorInstaller());

            var provider = new WindsorFilterAttributeFilterProvider(this.Container);
            FilterProviders.Providers.Add(provider);

            DependencyResolver.SetResolver(new WindsorDependencyResolver(ServiceFactory.Container));
        }

        public IWindsorContainer Container
        {
            get { return ServiceFactory.Container; }
        }
    }
}