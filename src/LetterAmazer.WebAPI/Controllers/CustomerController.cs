using LetterAmazer.Business.Services.Domain.Customers;
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
    public class CustomerController : ApiController
    {
        private ICustomerService customerService;
        public CustomerController(ICustomerService _customerService)
        {
            this.customerService = _customerService;
        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="CustomerDTO"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Create")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public CustomerDTO Create(CustomerDTO customerDTO)
        {
            var newCustomer = this.customerService.Create(AutoMapper.Mapper.Map<CustomerDTO, Customer>(customerDTO));
            return AutoMapper.Mapper.Map<Customer, CustomerDTO>(newCustomer);
        }

        /// <summary>
        /// Update Custome
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Update")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public CustomerDTO Update(CustomerDTO customerDTO)
        {
            var updatedCustomer = this.customerService.Update(AutoMapper.Mapper.Map<CustomerDTO, Customer>(customerDTO));
            return AutoMapper.Mapper.Map<Customer, CustomerDTO>(updatedCustomer);
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Delete")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public HttpResponseMessage Delete(int CustomerId)
        {
            var customer = this.customerService.GetCustomerById(CustomerId);
            this.customerService.Delete(customer);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}