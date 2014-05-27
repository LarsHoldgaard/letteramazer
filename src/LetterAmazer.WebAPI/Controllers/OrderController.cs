using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Data.DTO;
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
        private IOrganisationService orgnisationService;
        public OrderController(IOrderService orderService, IOrganisationService orgnisationService)
        {
            this.orderService = orderService;
            this.orgnisationService = orgnisationService;
        }
        //
        // 
        /// Create Create
        /// </summary>
        /// <param name="id">The item post id.</param>
        [HttpGet, ActionName("Create")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public HttpResponseMessage Test()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Create")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public OrderDTO Create(OrderDTO orderDTO)
        {
            var newOrder = this.orderService.Create(AutoMapper.Mapper.Map<OrderDTO,Order>(orderDTO));
            return AutoMapper.Mapper.Map<Order, OrderDTO>(newOrder);
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="orderDTO"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Update")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public OrderDTO Update(OrderDTO orderDTO)
        {
            var newOrder = this.orderService.Update(AutoMapper.Mapper.Map<OrderDTO, Order>(orderDTO));
            return AutoMapper.Mapper.Map<Order, OrderDTO>(newOrder);
        }

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Delete")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public HttpResponseMessage Delete(int orderid)
        {
            var order = this.orderService.GetOrderById(orderid);
            this.orderService.Delete(order);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
