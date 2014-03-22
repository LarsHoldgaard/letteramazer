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

        void SendInvoice(Order order, Domain.Invoice.Invoice invoice);

        void NotificationInvoiceCreated();

        void NotificationApiWish(string email, string organisation, string comment);
    }
}
