using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.Mails.ViewModels;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Utils;
using log4net;
using Newtonsoft.Json;

namespace LetterAmazer.Business.Services.Services
{
    public class MailService : IMailService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MailService));

        private string resetPasswordUrl;
        private string baseUrl;
        private string invoiceUrl;
        private string createUrl;

        private string mandrillApiUrl;
        private string mandrillApiKey;
        private string notificationEmail;

        public MailService()
        {
            this.baseUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.BasePath");
            this.createUrl = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Customer.Confirm");
            this.resetPasswordUrl = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Customer.ResetPassword");
            this.invoiceUrl = baseUrl + ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Invoice.ServiceUrl");
            this.mandrillApiUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Mail.Mandrill.ApiUrl");
            this.mandrillApiKey = ConfigurationManager.AppSettings.Get("LetterAmazer.Mail.Mandrill.ApiKey");
            this.notificationEmail = ConfigurationManager.AppSettings.Get("LetterAmazer.Notification.Emails");

        }

        private void SendTemplate(MandrillTemplateSend obj)
        {
            string key = "messages/send-template.json";
            obj.key = mandrillApiKey;
            string postUrl = string.Format("{0}/{1}", mandrillApiUrl, key);

            var jsonContent = JsonConvert.SerializeObject(obj);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonContent);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception exp)
            {
                throw;
            }

        }

        public void ResetPassword(Customer customer)
        {
            var template_name = "letteramazer-customer-forgotpassword";

            var model = new MandrillTemplateSend();
            model.template_name = template_name;
            model.message.merge = true;
            model.message.to.Add(new To()
            {
                email = customer.Email
            });
            model.message.merge_vars.Add(new Merge_Vars()
            {
                rcpt = customer.Email,
                vars = new List<Var>()
                {
                    new Var()
                    {
                        name = "RESETPASSWORD",
                        content =string.Format(resetPasswordUrl,customer.ResetPasswordKey)
                    }
                }
            });

            SendTemplate(model);
        }

        public void ConfirmUser(Customer customer)
        {
            var template_name = "letteramazer-registration-organisation";

            var model = new MandrillTemplateSend();
            model.template_name = template_name;
            model.message.merge = true;
            model.message.to.Add(new To()
            {
                email = customer.Email
            });
            model.message.merge_vars.Add(new Merge_Vars()
            {
                rcpt = customer.Email,
                vars = new List<Var>()
                {
                    new Var()
                    {
                        name = "REGISTERLINK",
                        content =string.Format(createUrl,customer.RegisterKey) 
                    }
                }
            });

            SendTemplate(model);
        }

        public void SendLetter(Order order)
        {
            var template_name = "letteramazer.letters.send";

            var model = new MandrillTemplateSend();
            model.template_name = template_name;
            model.message.merge = false;
            model.message.to.Add(new To()
            {
                email = order.Customer.Email
            });
            model.message.merge_vars.Add(new Merge_Vars()
            {
                rcpt = order.Customer.Email,
            });

            SendTemplate(model);
        }

        public void SendInvoice(Order order, Invoice invoice)
        {
            var template_name = "letteramazer.customer.invoice_created";

            var model = new MandrillTemplateSend();
            model.template_name = template_name;
            model.message.merge = true;
            model.message.to.Add(new To()
            {
                email = order.Customer.Email
            });
            model.message.merge_vars.Add(new Merge_Vars()
            {
                rcpt = order.Customer.Email,
                vars = new List<Var>()
                {
                    new Var()
                    {
                        name = "INVOICELINK",
                        content =string.Format(invoiceUrl,invoice.Guid) 
                    }
                }
            });
            SendTemplate(model);
        }

        public void NotificationInvoiceCreated()
        {
            var template_name = "letteramazer.notification.invoice_created";

            var model = new MandrillTemplateSend();
            model.template_name = template_name;
            model.message.merge = false;
            model.message.to.Add(new To()
            {
                email = notificationEmail
            });

            SendTemplate(model);
        }
    
    
    }

}
