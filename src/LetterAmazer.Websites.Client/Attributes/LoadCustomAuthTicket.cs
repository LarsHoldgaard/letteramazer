using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Utils.Helpers;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Asn1.Ocsp;

namespace LetterAmazer.Websites.Client.Attributes
{
    public class LoadCustomAuthTicket : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string userId = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        int userIdInt = 0;
                        int.TryParse(userId, out userIdInt);

                        if (userIdInt > 0)
                        {
                            var service = ServiceFactory.Get<ICustomerService>();
                            
                            var customer = service.GetCustomerById(userIdInt);
                            if (customer != null)
                            {
                                SessionHelper.Customer = customer;
                            }
                        }

                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}