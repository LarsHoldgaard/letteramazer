using System.Web;
using System.Web.Mvc;
using LetterAmazer.Websites.Client.Attributes;

namespace LetterAmazer.Websites.Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoadCustomAuthTicket());
        }
    }
}