namespace LetterAmazer.Business.Services.Domain.Customers
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
        bool IsValidCredits(int userId, decimal price);
        decimal GetAvailableCredits(int userId);
    }
}
