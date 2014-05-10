using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.Content;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Factory.Interfaces
{
    public interface IContentFactory
    {
        CmsContent Create(DbCmsContent content);
        List<CmsContent> Create(List<DbCmsContent> content);
    }
}
