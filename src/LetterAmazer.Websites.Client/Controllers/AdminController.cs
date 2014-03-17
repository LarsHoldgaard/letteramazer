using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.ViewModels.Admin;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class AdminController : Controller
    {
        private IDeliveryJobService deliveryJobService;

        public AdminController(IDeliveryJobService deliveryJobService)
        {
            this.deliveryJobService = deliveryJobService;
        }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            if (SessionHelper.Customer == null)
            {
                RedirectToAction("Index", "Home");
            }

            return View(new AdminDefaultViewModel());
        }

        [HttpPost]
        public ActionResult Index(AdminDefaultViewModel adminDefaultViewModel)
        {
            deliveryJobService.Execute();

            return View(new AdminDefaultViewModel());
        }

    }
}
