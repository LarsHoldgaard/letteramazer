using LetterAmazer.Business.Services.Domain.Customers;
using System.IO;
using LetterAmazer.Business.Services.Domain.Notifications;
using LetterAmazer.Business.Services.Domain.Notifications.Mail;

namespace LetterAmazer.Business.Services.Services
{
    public class NotificationService : INotificationService
    {
        private IEmailSender emailSender;
        private string fromMail;
        private string emailTemplatesFolder;

        public NotificationService(string fromMail, string emailTemplatesFolder, IEmailSender emailSender)
        {
            this.fromMail = fromMail;
            this.emailSender = emailSender;
            this.emailTemplatesFolder = emailTemplatesFolder;
        }

        public void SendResetPasswordUrl(string resetPasswordUrl, Customer user)
        {
            string emailTemplatePath = Path.Combine(this.emailTemplatesFolder, string.Format("{0}.xml", "ResetPasswordUrl"));

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add("FromMail", this.fromMail);
            parameters.Add("ToMail", user.Email);
            parameters.Add("Name", user.CustomerInfo.FirstName);
            parameters.Add("Url", resetPasswordUrl);

            EmailTemplate emailTemplate = EmailUtility.ComposeTemplate(emailTemplatePath, parameters);
            emailSender.Send(emailTemplate, new BypassEmailFormatter(), null);
        }

        public void SendMembershipInformation(Customer user)
        {
            string emailTemplatePath = Path.Combine(this.emailTemplatesFolder, string.Format("{0}.xml", "MembershipInformation"));

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add("FromMail", this.fromMail);
            parameters.Add("ToMail", user.Email);
            parameters.Add("Email", user.Email);
            parameters.Add("Password", user.Password);

            EmailTemplate emailTemplate = EmailUtility.ComposeTemplate(emailTemplatePath, parameters);
            emailSender.Send(emailTemplate, new BypassEmailFormatter(), null);
        }

        private class BypassEmailFormatter : IEmailFormatter
        {
            public EmailTemplate Format(EmailTemplate template, ParameterCollection parameters)
            {
                return template;
            }
        }
    }
}
