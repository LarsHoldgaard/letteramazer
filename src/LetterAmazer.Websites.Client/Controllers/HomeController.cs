using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (!(Request.Url.AbsolutePath.Contains("/en") || Request.Url.AbsolutePath.Contains("/da")))
            {
                Response.Redirect("~/en");
            }
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult Business()
        {
            return View();
        }
        public ActionResult Api()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Pricing()
        {
            return View();
        }



    }
}
