using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.WebAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LetterAmazer.WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderController : ApiController
    {

        public OrderController()
        {

        }
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
