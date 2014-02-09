namespace LetterAmazer.Business.Services.Domain.Notifications.Mail
{
    public interface IEmailSender
    {
        void Send(EmailTemplate template, IEmailFormatter formatter, ParameterCollection parameterCollection);
    }
}
