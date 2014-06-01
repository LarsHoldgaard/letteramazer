using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Orders;

namespace LetterAmazer.Business.Services.Domain.Mails
{
    public interface IMailService
    {
        void ResetPassword(Customer customer);
        void ConfirmUser(Customer customer);
        void SendLetter(Order order);

        void SendInvoice(Order order, Invoice.Invoice invoice);

        void NotificationInvoiceCreated();
        void SendIntermailStatus(string status, int letters);

        void NotificationApiWish(string email, string organisation, string comment);
        void NotificationResellerWish(string email, string wish, string comment);

        void NotificationNewOrder(string amount);
        void NotificationNewUser(string email);
        void NotificationTryService(string company, string email, string country, string letterCount, string phone);

    }

}
