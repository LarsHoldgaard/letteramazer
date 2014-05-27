using System.Web.Mvc;
using Castle.Windsor;
using System;
using System.Web.Routing;
using System.Web;
using System.Collections.Generic;

namespace LetterAmazer.WebAPI.IoC
{
    /// <summary>
    /// Controller Factory class for instantiating controllers using the Windsor IoC container.
    /// </summary>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
       
        private readonly IWindsorContainer container = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public WindsorControllerFactory(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404,
                    string.Format("The controller for path '{0}' could not be found or it does not implement IController.",
                    requestContext.HttpContext.Request.Path));
            }

            IController controller = (IController)container.Resolve(controllerType);
            return controller;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public override void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }

            container.Release(controller);
        }
    }
}