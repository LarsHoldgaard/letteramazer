using System.Collections.Generic;

namespace LetterAmazer.Business.Services.Domain.Customers
{
    public interface ICustomerService
    {
        
        Customer Create(Customer customer);
        Customer Update(Customer customer);
        void Delete(Customer customer);
        Customer GetCustomerById(int customerId);
        Customer LoginUser(string email, string password);
        
        List<Customer> GetCustomerBySpecification(CustomerSpecification specification);

        void RecoverPassword(string email);
        void ActivateUser(Customer customer);
        
    }
}
