using System.Collections.Generic;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface ICustomerFactory
    {
        ICountryService CountryService { get; set; }
        Customer Create(DbCustomers dbCustomer);
        List<Customer> Create(List<DbCustomers> dbcustomers);


    }
}