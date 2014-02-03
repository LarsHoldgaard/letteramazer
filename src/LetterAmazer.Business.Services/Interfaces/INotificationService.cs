using LetterAmazer.Business.Services.Domain.Customers;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface INotificationService
    {
        void SendResetPasswordUrl(string resetPasswordUrl, Customer user);
        void SendMembershipInformation(Customer customer);
    }
}
