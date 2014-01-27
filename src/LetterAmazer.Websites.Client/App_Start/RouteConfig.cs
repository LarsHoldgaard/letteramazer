using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LetterAmazer.Websites.Client.App_Start;
using LetterAmazer.Websites.Client.Helpers;

namespace LetterAmazer.Websites.Client
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SingleLetterPaypalIPN",
                url: "SingleLetter/PaypalIpn/{id}",
                defaults: new { controller = "SingleLetter", action = "PaypalIpn", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SingleLetter",
                url: "SingleLetter/{action}/{id}",
                defaults: new { controller = "SingleLetter", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "User",
                url: "User/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            foreach (Route r in routes)
            {
                if (!(r.RouteHandler is SingleCultureMvcRouteHandler))
                {
                    r.RouteHandler = new MultiCultureMvcRouteHandler();
                    r.Url = "{culture}/" + r.Url;

                    if (r.Defaults == null)
                    {
                        r.Defaults = new RouteValueDictionary();
                    }
                    r.Defaults.Add("culture", Culture.en.ToString());

                    //Adding constraint for culture param
                    if (r.Constraints == null)
                    {
                        r.Constraints = new RouteValueDictionary();
                    }
                    r.Constraints.Add("culture", new CultureConstraint(Culture.en.ToString(), Culture.da.ToString()));
                }
            }

        }
    }
}