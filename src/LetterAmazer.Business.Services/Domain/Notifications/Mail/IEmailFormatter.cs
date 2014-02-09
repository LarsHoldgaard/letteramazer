
namespace LetterAmazer.Business.Services.Domain.Notifications.Mail
{
    public interface IEmailFormatter
    {
        EmailTemplate Format(EmailTemplate template, ParameterCollection parameterHash);
    }
}
