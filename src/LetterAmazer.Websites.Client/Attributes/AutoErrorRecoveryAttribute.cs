using LetterAmazer.Websites.Client.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.Attributes
{
    public class AutoErrorRecoveryAttribute : ActionFilterAttribute
    {
        public string ViewName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TempDataDictionary TempData = filterContext.Controller.TempData;
            if (TempData["LastError"] == null)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            string errorKey = filterContext.Controller.TempData["LastError"] as string;

            ViewResult result = new ViewResult();
            result.ViewData.Model = filterContext.Controller.TempData["Model" + errorKey];
            if (filterContext.Controller is BaseController)
            {
                result.ViewData.Model = (filterContext.Controller as BaseController).InitializeViewModel(result.ViewData.Model);
            }
            ModelStateDictionary previousModelState = (ModelStateDictionary)TempData["ModelState" + errorKey];
            foreach (string key in previousModelState.Keys)
            {
                if (!result.ViewData.ModelState.ContainsKey(key))
                {
                    result.ViewData.ModelState.Add(key, previousModelState[key]);
                }
            }
            if (String.IsNullOrEmpty(this.ViewName))
            {
                result.ViewName = filterContext.ActionDescriptor.ActionName;
            }
            else
            {
                result.ViewName = this.ViewName;
            }

            filterContext.Result = result;
            base.OnActionExecuting(filterContext);
        }
    }
}