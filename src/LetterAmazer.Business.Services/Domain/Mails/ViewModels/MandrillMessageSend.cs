using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Mails.ViewModels
{
    public class MandrillMessageSend
    {
        public string html { get; set; }
        public string text { get; set; }
        public string subject { get; set; }
        public string from_email { get; set; }
        public string from_name { get; set; }
        public List<To> to { get; set; }
        public Headers headers { get; set; }
        public bool important { get; set; }
        public object track_opens { get; set; }
        public object track_clicks { get; set; }
        public object auto_text { get; set; }
        public object auto_html { get; set; }
        public object inline_css { get; set; }
        public object url_strip_qs { get; set; }
        public object preserve_recipients { get; set; }
        public object view_content_link { get; set; }
        public string bcc_address { get; set; }
        public object tracking_domain { get; set; }
        public object signing_domain { get; set; }
        public object return_path_domain { get; set; }
        public bool merge { get; set; }
        public List<Global_Merge_Vars> global_merge_vars { get; set; }
        public List<Merge_Vars> merge_vars { get; set; }
        public string[] tags { get; set; }
        public string subaccount { get; set; }
        public string[] google_analytics_domains { get; set; }
        public string google_analytics_campaign { get; set; }
        public Metadata metadata { get; set; }
        public List<Recipient_Metadata> recipient_metadata { get; set; }
        public List<Attachment> attachments { get; set; }
        public List<Image> images { get; set; }

        public MandrillMessageSend()
        {
            this.global_merge_vars =new List<Global_Merge_Vars>();
            this.merge_vars = new List<Merge_Vars>();
            this.to = new List<To>();
            this.attachments = new List<Attachment>();
            this.images = new List<Image>();
        }
    }

    public class Headers
    {
        public string ReplyTo { get; set; }
    }

    public class Metadata
    {
        public string website { get; set; }
    }

    public class To
    {
        public string email { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Global_Merge_Vars
    {
        public string name { get; set; }
        public string content { get; set; }
    }

    public class Merge_Vars
    {
        public string rcpt { get; set; }
        public List<Var> vars { get; set; }

        public Merge_Vars()
        {
            this.vars = new List<Var>();
        }
    }

    public class Var
    {
        public string name { get; set; }
        public string content { get; set; }
    }

    public class Recipient_Metadata
    {
        public string rcpt { get; set; }
        public Values values { get; set; }
    }

    public class Values
    {
        public int user_id { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public string name { get; set; }
        public string content { get; set; }
    }

    public class Image
    {
        public string type { get; set; }
        public string name { get; set; }
        public string content { get; set; }
    }

    public class Template_Content
    {
        public string name { get; set; }
        public string content { get; set; }
    }



}
