
namespace LetterAmazer.Business.Services.Components.Mail
{
    public interface IEmailFormatter
    {
        EmailTemplate Format(EmailTemplate template, ParameterCollection parameterHash);
    }
}
