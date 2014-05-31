using System.Collections.Generic;
using System.Web.Mvc;
using Castle.Windsor;
namespace LetterAmazer.WebAPI.IoC
{
    public class WindsorFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IWindsorContainer container = null;

        public WindsorFilterAttributeFilterProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetControllerAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                this.container.InjectProperties(attribute, true);
            }

            return attributes;
        }

        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetActionAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                this.container.InjectProperties(attribute, true);
            }

            return attributes;
        }
    }
}