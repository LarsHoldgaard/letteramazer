﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Services.Partners.Invoice;
using LetterAmazer.Data.Repository.Data;
using LetterAmazer.WebAPI.Common;
using LetterAmazer.WebAPI.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LetterAmazer.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitializeContainer();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void InitializeContainer()
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);

            Container.Register(Component.For<IWindsorContainer>().Instance(this.Container));

            Container.Install(new WebWindsorInstaller());

            registerCustom();

            var provider = new WindsorFilterAttributeFilterProvider(this.Container);
            FilterProviders.Providers.Add(provider);

            DependencyResolver.SetResolver(new WindsorDependencyResolver(ServiceFactory.Container));


            // register WebApi controllers
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorHttpControllerActivator(ServiceFactory.Container));
        }
        private void registerCustom()
        {
            // All services in service DLL
            var assembly = Assembly.LoadFrom(Server.MapPath("~/bin/LetterAmazer.Business.Services.dll"));
            ;
            Container.Register(
                Classes.FromAssembly(assembly)
                    .InNamespace("LetterAmazer.Business.Services.Services")
                    .WithServiceAllInterfaces());

            Container.Register(
                Classes.FromAssembly(assembly)
                    .InNamespace("LetterAmazer.Business.Services.Services.OrderService")
                    .WithServiceAllInterfaces());

            Container.Register(
              Classes.FromAssembly(assembly)
                  .InNamespace("LetterAmazer.Business.Services.Services.CustomerService")
                  .WithServiceAllInterfaces());

            Container.Register(
              Classes.FromAssembly(assembly)
                  .InNamespace("LetterAmazer.Business.Services.Services.InvoiceService")
                  .WithServiceAllInterfaces());

            Container.Register(
                Classes.FromAssembly(assembly)
                    .InNamespace("LetterAmazer.Business.Services.Services.OrganisationService")
                    .WithServiceAllInterfaces());


            // All factories in service DLL
            Container.Register(
                Classes.FromAssembly(assembly)
                    .InNamespace("LetterAmazer.Business.Services.Factory")
                    .WithServiceAllInterfaces());

            Container.Register(Component.For<EconomicInvoiceService>());
            Container.Register(Component.For<LetterAmazerEntities>());
        }

        /// <summary>
        /// Catch and handle unhandled exceptions (all application layers) via the
        /// Enterprise Library Exception Handling Block.
        /// See web.config for exception policy details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();

            var exception = new ServerException
            {
                ExceptionTime = DateTime.Now,
                Message = "An unhandled exception occurred" + ex.Message + "Inner exception: " + ex.InnerException,
                StackTrace = ex.StackTrace,
            };

            throw new UserFriendlyException("Something went wrong!",ex.InnerException);
        }

        public IWindsorContainer Container
        {
            get { return ServiceFactory.Container; }
        }
    }
}
