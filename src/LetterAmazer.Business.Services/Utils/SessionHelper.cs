using System.Web;
using LetterAmazer.Business.Services.Domain.Customers;

namespace LetterAmazer.Business.Utils.Helpers
{
    public static class SessionHelper
    {
        public static Customer Customer
        {
            get
            {
                return HttpContext.Current.Session["customer"] as Customer;
            }
            set
            {
                HttpContext.Current.Session["customer"] = value;
            }
        }

    }
}
