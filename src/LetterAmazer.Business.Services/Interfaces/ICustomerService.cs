using LetterAmazer.Business.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface ICustomerService
    {
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        Customer GetCustomerById(int customerId);
        Customer GetUserByEmail(string email);
        void DeleteCustomer(int customerId);
        void RecoverPassword(string email);
        void ChangePassword(string email, string oldPassword, string newPassword);
        Customer ValidateUser(string email, string password);
        Customer GetUserByResetPasswordKey(string resetPasswordKey);
        void ResetPassword(string resetPasswordKey, string newPassword);
    }
}
