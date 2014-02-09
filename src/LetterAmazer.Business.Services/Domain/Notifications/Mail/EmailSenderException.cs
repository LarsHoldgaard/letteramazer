using System;

namespace LetterAmazer.Business.Services.Domain.Notifications.Mail
{
	/// <summary>
	/// Description of EmailSenderException.
	/// </summary>
	public class EmailSenderException : Exception
	{
		public EmailSenderException()
		{
		}
		
		public EmailSenderException(string message) : base(message)
		{
		}
	}
}
