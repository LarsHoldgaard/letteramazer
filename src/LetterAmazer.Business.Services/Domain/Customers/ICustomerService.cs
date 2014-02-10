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

        //Customer GetUserByEmail(string email);
        //void DeleteCustomer(int customerId);
        
        //void ChangePassword(string email, string oldPassword, string newPassword);
        //Customer ValidateUser(string email, string password);
        //Customer GetUserByResetPasswordKey(string resetPasswordKey);
        //void ResetPassword(string resetPasswordKey, string newPassword);
        //bool IsValidCredits(int userId, decimal price);
        //decimal GetAvailableCredits(int userId);
    }
}
