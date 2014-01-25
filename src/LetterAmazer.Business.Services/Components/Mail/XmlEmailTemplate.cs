using System.Text;
using System.Xml;

namespace LetterAmazer.Business.Services.Components.Mail
{
    public class XmlEmailTemplate : EmailTemplate
    {
        public XmlEmailTemplate(string xmlTemplateFile)
        {
            XmlDocument xmlTemplate = new XmlDocument();
            xmlTemplate.Load(xmlTemplateFile);
            Init(xmlTemplate);
        }

        public XmlEmailTemplate(XmlDocument xmlTemplate)
        {
            Init(xmlTemplate);
        }

        private void Init(XmlDocument xmlTemplate)
        {
            //XmlElement root = xmlTemplate.DocumentElement;
            this.subject = xmlTemplate.SelectSingleNode("/email-template/subject").InnerText.Trim();
            this.from = xmlTemplate.SelectSingleNode("/email-template/addresses/entry[@name='From']").Attributes["value"].InnerText;
            this.to = xmlTemplate.SelectSingleNode("/email-template/addresses/entry[@name='To']").Attributes["value"].InnerText;
            this.body = xmlTemplate.SelectSingleNode("/email-template/body").InnerText.Trim();
            this.bodyContentType = xmlTemplate.SelectSingleNode("/email-template/body/@format").InnerText;
            string bodyEncodingName = xmlTemplate.SelectSingleNode("/email-template/body/@encoding").InnerText;
            try { this.bodyEncoding = Encoding.GetEncoding(bodyEncodingName); }
            catch { this.bodyEncoding = Encoding.GetEncoding("iso-8859-1"); }
        }
    }
}
