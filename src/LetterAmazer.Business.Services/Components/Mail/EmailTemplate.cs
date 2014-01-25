using System;
using System.Text;

namespace LetterAmazer.Business.Services.Components.Mail
{
    public class EmailTemplate
    {
        #region Fields
        protected string subject;
        protected string from;
        protected string to;
        protected string body;
        protected Encoding bodyEncoding;
        protected string bodyContentType;
        #endregion Fields

        #region Constructors
        public EmailTemplate()
        {
        }

        public EmailTemplate(string subject, string from, string to, string body, string bodyContentType, Encoding bodyEncoding)
        {
            this.subject = subject;
            this.from = from;
            this.to = to;
            this.body = body;
            this.bodyEncoding = bodyEncoding;
            this.bodyContentType = bodyContentType;
        }
        #endregion Constructors

        #region Properties
        public string Subject
        {
            get { return subject; }
        }
        public string From
        {
            get { return from; }
        }
        public string To
        {
            get { return to; }
        }
        public string Body
        {
            get { return body; }
        }
        public string BodyContentType
        {
            get { return bodyContentType; }
        }
        public Encoding BodyEncoding
        {
            get { return bodyEncoding; }
        }
        #endregion Properties

        public override string ToString()
        {
            return String.Format(@"
                From: {0}
                To: {1}
                Subject: {2}
                Body Content Type: {3}
                Body Encoding : {4}    
                Body: {5}", From, To, Subject, BodyContentType, BodyEncoding, Body);
        }
    }
}
