using LetterAmazer.Business.Services;
using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LetterAmazer.Websites.Client.Helpers
{
    public class SecurityHelper
    {
        private static Random random = new Random();
        private static String passwordChars = "0123456789abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ!@#$*";

        public static String GeneratePassword(int numberOfChars)
        {
            StringBuilder sb = new StringBuilder(numberOfChars);
            for (int i = 0; i < numberOfChars; i++)
            {
                sb.Append(passwordChars[random.Next(passwordChars.Length)]);
            }
            return sb.ToString();
        }

        public static Customer CurrentUser
        {
            get
            {
                HttpContext currentContext = HttpContext.Current;
                if (currentContext == null) return null;
                if (!currentContext.User.Identity.IsAuthenticated) return null;

                if (currentContext.Items["CurrentUser"] != null)
                {
                    return (Customer)currentContext.Items["CurrentUser"];
                }
                Customer user = ServiceFactory.Get<ICustomerService>().GetCustomerById(System.Convert.ToInt32(currentContext.User.Identity.Name));
                currentContext.Items["CurrentUser"] = user;
                return user;
            }
        }

        public static bool IsAuthenticated
        {
            get { return CurrentUser != null; }
        }
    }
}
