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
                name: "PaypalIPN",
                url: "Callback/PaypalIpn/{id}",
                defaults: new { controller = "Callback", action = "PaypalIpn", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                            name: "BitPayIPN",
                            url: "Callback/BitPay/{id}",
                            defaults: new { controller = "Callback", action = "BitPay", id = UrlParameter.Optional }
                        );
            routes.MapRoute(
                            name: "EpayIPN",
                            url: "Callback/Epay/{id}",
                            defaults: new { controller = "Callback", action = "Epay", id = UrlParameter.Optional }
                        );


            routes.MapRoute(
                name: "SingleLetter",
                url: "SingleLetter/{action}/{id}",
                defaults: new { controller = "SingleLetter", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Invoice",
                url: "payment/{action}/{id}",
                defaults: new { controller = "Payment", action = "Invoice", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Partners",
                url: "partner/{action}/{id}",
                defaults: new { controller = "Partner", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "User",
                url: "User/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                          name: "Pricing",
                          url: "pricing/{alias}",
                          defaults: new { controller = "Home", action = "GetPricing" }
                      );

            routes.MapRoute(
                        name: "SendLetters",
                        url: "sendletters/{alias}",
                        defaults: new { controller = "Home", action = "GetSendALetterTo" }
                    );



            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}