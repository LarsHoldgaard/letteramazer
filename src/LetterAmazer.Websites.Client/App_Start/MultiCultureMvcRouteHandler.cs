using LetterAmazer.Websites.Client.Helpers;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LetterAmazer.Websites.Client.App_Start
{
    //public class MultiCultureMvcRouteHandler : MvcRouteHandler
    //{
    //    protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
    //    {
    //        var culture = requestContext.RouteData.Values["culture"].ToString();
    //        var ci = new CultureInfo(Culture.en.ToString() == culture ? "en-US" : "da-DK");
    //        Thread.CurrentThread.CurrentUICulture = ci;
    //        Thread.CurrentThread.CurrentCulture = ci;
    //        return base.GetHttpHandler(requestContext);
    //    }
    //}
}