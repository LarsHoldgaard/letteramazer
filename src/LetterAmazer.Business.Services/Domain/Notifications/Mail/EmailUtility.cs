using System.Xml;

namespace LetterAmazer.Business.Services.Domain.Notifications.Mail
{
    public class EmailUtility
    {
        /// <summary>
        /// Sends the XML mail.
        /// </summary>
        /// <param name="xmlTemplateFileName">Name of the XML template file.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sender">The sender.</param>
        public static void SendXmlMail(string xmlTemplateFileName, ParameterCollection parameters, IEmailSender sender)
        {
            EmailTemplate template = new XmlEmailTemplate(xmlTemplateFileName);
            IEmailFormatter formatter = new XmlTemplateEmailFormatter();
            sender.Send(template, formatter, parameters);
        }

        /// <summary>
        /// Sends the XML mail.
        /// </summary>
        /// <param name="xmlTemplate">The XML template.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sender">The sender.</param>
        public static void SendXmlMail(XmlDocument xmlTemplate, ParameterCollection parameters, IEmailSender sender)
        {
            EmailTemplate template = new XmlEmailTemplate(xmlTemplate);
            IEmailFormatter formatter = new XmlTemplateEmailFormatter();
            sender.Send(template, formatter, parameters);
        }


        /// <summary>
        /// Sends the XML mail using string template formatter.
        /// </summary>
        /// <param name="xmlTemplateFileName">Name of the XML template file.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sender">The sender.</param>
        public static void SendXmlMailUsingStringTemplateFormatter(string xmlTemplateFileName, ParameterCollection parameters, IEmailSender sender)
        {
            EmailTemplate template = new XmlEmailTemplate(xmlTemplateFileName);
            IEmailFormatter formatter = new StringTemplateEmailFormatter();
            sender.Send(template, formatter, parameters);
        }

        /// <summary>
        /// Sends the XML mail using string template formatter.
        /// </summary>
        /// <param name="xmlTemplate">The XML template.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sender">The sender.</param>
        public static void SendXmlMailUsingStringTemplateFormatter(XmlDocument xmlTemplate, ParameterCollection parameters, IEmailSender sender)
        {
            EmailTemplate template = new XmlEmailTemplate(xmlTemplate);
            IEmailFormatter formatter = new StringTemplateEmailFormatter();
            sender.Send(template, formatter, parameters);
        }

        public static EmailTemplate ComposeTemplate(string emailTemplatePath, ParameterCollection parameters)
        {
            XmlEmailTemplate xmlTemplate = new XmlEmailTemplate(emailTemplatePath);
            StringTemplateEmailFormatter formatter = new StringTemplateEmailFormatter();
            return formatter.Format(xmlTemplate, parameters);
        }
    }
}
