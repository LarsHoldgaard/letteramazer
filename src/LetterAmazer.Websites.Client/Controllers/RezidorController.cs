using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace LetterAmazer.Websites.Client.Controllers
{
    /// <summary>
    /// TODO: Delete this crap
    /// </summary>
    public class RezidorController : BaseController
    {
        public ActionResult Index()
        {

            return Redirect("http://phonefundraising.net/rezidor/index.php");
        }
    }
}
