using LetterAmazer.Business.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Interfaces
{
    public interface INotificationService
    {
        void SendResetPasswordUrl(string resetPasswordUrl, Customer user);
        void SendMembershipInformation(Customer customer);
    }
}
