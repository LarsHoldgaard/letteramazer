using LetterAmazer.Business.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class BaseController : Controller
    {
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (!(Request.Url.AbsolutePath.Contains("/en") || Request.Url.AbsolutePath.Contains("/da")))
        //    {
        //        filterContext.Result = new RedirectResult("~/en/" + Request.Url.PathAndQuery);
        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        public virtual ActionResult RedirectToActionWithError(string actionName, object model)
        {
            return RedirectToActionWithError(actionName, model, null);
        }

        public virtual ActionResult RedirectToActionWithError(string actionName, object model, object routeValues)
        {
            string errorKey = GenerateUniqueKey();
            TempData["LastError"] = errorKey;
            TempData["ModelState" + errorKey] = ModelState;
            TempData["Model" + errorKey] = model;
            return RedirectToAction(actionName, routeValues);
        }

        public bool IsRedirectedFromError()
        {
            return TempData["LastError"] != null;
        }

        public void RecoverFromError()
        {
            string errorKey = (string)TempData["LastError"];
            ModelStateDictionary previousModelState = (ModelStateDictionary)TempData["ModelState" + errorKey];
            foreach (string key in previousModelState.Keys)
            {
                if (!ModelState.ContainsKey(key))
                {
                    ModelState.Add(key, previousModelState[key]);
                }
            }
        }

        public T RecoverFromError<T>()
        {
            RecoverFromError();
            string errorKey = (string)TempData["LastError"];
            return (T)TempData["Model" + errorKey];
        }

        protected string GenerateUniqueKey()
        {
            int maxSize = 8;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        public void ValidateInput()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                foreach (var item in errors)
                {
                    foreach (ModelError error in item)
                    {
                        throw new BusinessException(error.ErrorMessage);
                    }
                }
            }
        }

        public object InitializeViewModel(object model)
        {
            return model;
        }
    }
}
