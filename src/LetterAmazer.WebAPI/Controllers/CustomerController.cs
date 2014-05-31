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
        [HttpPost, ActionName("Create")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public CustomerDTO Create(CustomerDTO customerDTO)
        {
            var newCustomer = this.customerService.Create(AutoMapper.Mapper.DynamicMap<CustomerDTO, Customer>(customerDTO));
            return AutoMapper.Mapper.DynamicMap<Customer, CustomerDTO>(newCustomer);
        }

        /// <summary>
        /// Update Custome
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Update")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public CustomerDTO Update(CustomerDTO customerDTO)
        {
            var updatedCustomer = this.customerService.Update(AutoMapper.Mapper.DynamicMap<CustomerDTO, Customer>(customerDTO));
            return AutoMapper.Mapper.DynamicMap<Customer, CustomerDTO>(updatedCustomer);
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public HttpResponseMessage Delete(int CustomerId)
        {
            var customer = this.customerService.GetCustomerById(CustomerId);
            this.customerService.Delete(customer);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Find Customer
        /// </summary>
        /// <param name="CustomerSpecificationDTO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Find")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public IList<CustomerDTO> Find(CustomerSpecificationDTO customerSpecification)
        {
            var customerList = this.customerService.GetCustomerBySpecification(AutoMapper.Mapper.DynamicMap<CustomerSpecificationDTO, CustomerSpecification>(customerSpecification));
            var customerListDto = new List<CustomerDTO>();
            foreach (var item in customerList)
            {
                customerListDto.Add(AutoMapper.Mapper.DynamicMap<CustomerDTO>(item));
            }
            return customerListDto;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <param name="CustomerSpecificationDTO"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Count")]
        [CustomAuthorize(Roles = "Admin, Super User")]
        public int Count(CustomerSpecificationDTO customerSpecification)
        {
            throw new NotImplementedException();
        }
    }
}