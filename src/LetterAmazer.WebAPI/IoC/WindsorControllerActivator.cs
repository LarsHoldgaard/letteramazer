using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LetterAmazer.WebAPI.IoC
{
    /// <summary>
    /// Reference: http://stackoverflow.com/questions/4140860/castle-windsor-dependency-resolver-for-mvc-3
    /// </summary>
    public class WindsorControllerActivator : IControllerActivator
    {
        private readonly IWindsorContainer container = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public WindsorControllerActivator(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            IController controller = (IController)container.Resolve(controllerType);
            return controller;
        }
    }
}