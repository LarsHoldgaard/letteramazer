using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Mails.ViewModels
{
    public class MandrillTemplateSend
    {
       

        public string key { get; set; }
        public string template_name { get; set; }
        public List<Template_Content> template_content { get; set; }
        public MandrillMessageSend message { get; set; }
        public bool async { get; set; }
        public string ip_pool { get; set; }
        public string send_at { get; set; }

        public MandrillTemplateSend()
        {
            this.template_content = new List<Template_Content>();
            this.message = new MandrillMessageSend();
        }


        public class Template_Content
        {
            public string name { get; set; }
            public string content { get; set; }
        }

    }
}
