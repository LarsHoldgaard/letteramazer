using System;
using log4net;

namespace LetterAmazer.Business.Services.Components.Mail
{
	/// <summary>
	/// Description of DebugEmailSender.
	/// </summary>
	public class DebugEmailSender : IEmailSender
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DebugEmailSender));

        public void Send(EmailTemplate template, IEmailFormatter formatter, ParameterCollection parameterHash)
        {
            EmailTemplate formattedTemplate = formatter.Format(template, parameterHash);
            logger.DebugFormat(@"
                Send email: {0}
                To: {1},
                From: {2},
                
                {3}", formattedTemplate.Subject, formattedTemplate.To, formattedTemplate.From, formattedTemplate.Body);
        }
	}
}
