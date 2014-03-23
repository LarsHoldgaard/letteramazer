using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Content;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory
{
    public class ContentFactory:IContentFactory
    {
        public CmsContent Create(DbCmsContent content)
        {
            return new CmsContent()
            {
                Id = content.Id,
                Content = content.Description,
                DateCreated = content.DateCreated,
                DateModified = content.DateModified,
                Enabled = content.Enabled,
                Headline = content.Headline,
                MetaDescription = content.MetaDescription,
                SeoTitle = content.SeoTitle,
                Section = content.Section,
                CmsKey = content.CmsKey,
                Alias = content.Alias
            };
        }

        public List<CmsContent> Create(List<DbCmsContent> content)
        {
            return content.Select(Create).ToList();
        }
    }
}
