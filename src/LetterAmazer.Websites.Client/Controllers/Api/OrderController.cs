using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System;
using LetterAmazer.Websites.Client.Extensions;
using LetterAmazer.Websites.Client.ViewModels.Home;
using LetterAmazer.Websites.Client.ViewModels.Shared.Utils;
using LetterAmazer.Websites.Client.ViewModels.User;
using log4net;
using System.Web.Security;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Websites.Client.Common;

namespace LetterAmazer.Websites.Client.Controllers
{
    public class OrderController : ApiController
    {
        private IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        //
        // 
        /// Create Create
        /// </summary>
        /// <param name="id">The item post id.</param>
        [HttpGet, ActionName("Create")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public HttpResponseMessage Create()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
