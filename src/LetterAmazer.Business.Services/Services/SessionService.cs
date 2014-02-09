using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Session;

namespace LetterAmazer.Business.Services.Services
{
    public class SessionService:ISessionService
    {
        public Customer Customer
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
