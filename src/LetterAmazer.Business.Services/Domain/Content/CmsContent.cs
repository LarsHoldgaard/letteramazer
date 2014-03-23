using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Content
{
    public class CmsContent
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string Section { get; set; }
        public string Alias { get; set; }

        public string CmsKey { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Enabled { get; set; }
        public string SeoTitle { get; set; }
        public string MetaDescription { get; set; }

        public CmsContent()
        {
            this.Enabled = true;
        }
    }
}
