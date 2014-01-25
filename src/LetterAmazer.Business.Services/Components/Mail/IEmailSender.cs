namespace LetterAmazer.Business.Services.Components.Mail
{
    public interface IEmailSender
    {
        void Send(EmailTemplate template, IEmailFormatter formatter, ParameterCollection parameterCollection);
    }
}
