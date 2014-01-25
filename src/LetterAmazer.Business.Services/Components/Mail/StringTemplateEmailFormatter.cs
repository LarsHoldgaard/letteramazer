using System;
using System.Collections;
using System.Text;

namespace LetterAmazer.Business.Services.Components.Mail
{
	/// <summary>
	/// Description of StringTemplateEmailFormatter.
	/// </summary>
	public class StringTemplateEmailFormatter : IEmailFormatter
	{
		public StringTemplateEmailFormatter()
		{
		}
		
		public EmailTemplate Format(EmailTemplate template, ParameterCollection parameterCollection)
	    {
	        string from = this.FormatText(template.From, parameterCollection);
	        string to = this.FormatText(template.To, parameterCollection);
	        string subject = "=?UTF-8?B?" + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.FormatText(template.Subject, parameterCollection))) + "?=";
	        return new EmailTemplate(subject, from, to, this.FormatText(template.Body, parameterCollection), template.BodyContentType, template.BodyEncoding);
	    }
	
	    private string FormatText(string text, ParameterCollection parameterCollection)
	    {
	        Antlr.StringTemplate.StringTemplate template = new Antlr.StringTemplate.StringTemplate(text);
	        IDictionaryEnumerator enumerator = parameterCollection.GetEnumerator();
	        while (enumerator.MoveNext())
	        {
	            string key = enumerator.Key as string;
	            object obj2 = enumerator.Value;
	            template.SetAttribute(key, obj2);
	        }
	        return template.ToString();
	    }
	}
}
