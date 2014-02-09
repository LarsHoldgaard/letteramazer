using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LetterAmazer.Test.Business.Services.Services
{
    public class CustomerTest
    {
        private readonly ICustomerService customerService;

        public CustomerTest(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [TestMethod]
        public void GetCustomerBySpecification()
        {
            var customerSpecification = new CustomerSpecification();

            var customers = customerService.GetCustomerBySpecification(customerSpecification);

            Assert.IsNotNull(customers);
        }
    }
}
