using System;
using System.Collections;
using System.Text;
using log4net;

namespace LetterAmazer.Business.Services.Components.Mail
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlTemplateEmailFormatter : IEmailFormatter
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(XmlTemplateEmailFormatter));
        
        #region IEmailFormatter Members

        /// <summary>
        /// Formats the specified email template.
        /// </summary>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="parameterHash">The parameter hash.</param>
        /// <returns></returns>
        public EmailTemplate Format(EmailTemplate emailTemplate, ParameterCollection parameterHash)
        {
            string fromAddresses = this.FormatSection(emailTemplate.From, emailTemplate, parameterHash);
            string toAddresses = this.FormatSection(emailTemplate.To, emailTemplate, parameterHash);
            string subject = "=?UTF-8?B?" +
                Convert.ToBase64String(Encoding.UTF8.GetBytes(this.FormatSection(emailTemplate.Subject, emailTemplate, parameterHash))) +
                "?=";            
            string body = this.FormatSection(emailTemplate.Body, emailTemplate, parameterHash, true);
            EmailTemplate formattedEmailTemplate = new EmailTemplate(subject, fromAddresses, toAddresses, body, emailTemplate.BodyContentType, emailTemplate.BodyEncoding);            
            logger.Debug("Formatted email template: " + formattedEmailTemplate);
            return formattedEmailTemplate;
        }
        #endregion

        #region Private members
        /// <summary>
        /// Formats the section.
        /// </summary>
        /// <param name="sectionTemplate">The section template.</param>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="parameterCollection">The parameter collection.</param>
        /// <returns>String</returns>
        protected string FormatSection(string sectionTemplate, EmailTemplate emailTemplate, ParameterCollection parameterCollection)
        {
            return this.FormatSection(sectionTemplate, emailTemplate, parameterCollection, false);
        }

        /// <summary>
        /// Formats the section.
        /// </summary>
        /// <param name="sectionTemplate">The section template.</param>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="parameterHash">The parameter hash.</param>
        /// <param name="htmlEncode">if set to <c>true</c> [HTML encode].</param>
        /// <returns>String</returns>
        protected string FormatSection(string sectionTemplate, EmailTemplate emailTemplate, ParameterCollection parameterHash, bool htmlEncode)
        {
            string text = sectionTemplate;            
            IDictionaryEnumerator hashEnum = parameterHash.GetEnumerator();
            while (hashEnum.MoveNext())
            {
                object key = hashEnum.Key;
                object value = hashEnum.Value;
                if (key != null && value != null) text = FormatOneField(text, emailTemplate, key.ToString(), value.ToString(), htmlEncode);
            }
            return text;
        }

        /// <summary>
        /// Formats the one field.
        /// </summary>
        /// <param name="sectionTemplate">The section template.</param>
        /// <param name="emailTemplate">The email template.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="htmlEncode">if set to <c>true</c> [HTML encode].</param>
        /// <returns>String</returns>
        protected string FormatOneField(string sectionTemplate, EmailTemplate emailTemplate, string fieldName, string fieldValue, bool htmlEncode)
        {
            string token = string.Format("${{{0}}}", fieldName);
            string value = emailTemplate.BodyContentType.Equals("text/html") && htmlEncode ? System.Web.HttpUtility.HtmlEncode(fieldValue).Replace("\n", "<br />") : fieldValue;
            return sectionTemplate.Replace(token, value);
        }
        #endregion        
    }
}
