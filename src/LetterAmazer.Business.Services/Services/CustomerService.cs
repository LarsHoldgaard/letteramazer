using LetterAmazer.Business.Services.Data;
using LetterAmazer.Business.Services.Exceptions;
using LetterAmazer.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private IRepository repository;
        public CustomerService(IRepository repository)
        {
            this.repository = repository;
        }

        public Customer GetCustomerById(int customerId)
        {
            Customer customer = repository.GetById<Customer>(customerId);
            if (customer == null)
            {
                throw new ItemNotFoundException("Customer");
            }
            return customer;
        }
    }
}
