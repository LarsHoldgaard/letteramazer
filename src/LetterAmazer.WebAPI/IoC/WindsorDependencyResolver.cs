using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;

namespace LetterAmazer.WebAPI.IoC
{
    /// <summary>
    /// Reference: http://stackoverflow.com/questions/4140860/castle-windsor-dependency-resolver-for-mvc-3
    /// </summary>
    public class WindsorDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer container = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public WindsorDependencyResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.ResolveAll(serviceType).Cast<object>() : new object[] { };
        }
    }
}