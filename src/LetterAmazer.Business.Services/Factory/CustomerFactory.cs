using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class CustomerFactory
    {

        public Customer Create(DbCustomers dbCustomer)
        {
            var customer = new Customer()
            {
                //FirstName = dbCustomer.CustomerInfo_FirstName,
                //LastName = dbCustomer.CustomerInfo_LastName,
                Id = dbCustomer.Id,
                Email = dbCustomer.Email,
                ResetPasswordKey = dbCustomer.ResetPasswordKey,
                DateCreated = dbCustomer.DateCreated,
                DateModified = dbCustomer.DateUpdated,
                Credit = dbCustomer.Credits.HasValue ? dbCustomer.Credits.Value : 0.0m,
                CreditLimit = dbCustomer.CreditLimit
            };

            return customer;
        }
    }
}
