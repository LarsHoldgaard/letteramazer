using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Content
{
    public interface IContentService
    {
        CmsContent GetContentById(int id);

        List<CmsContent> GetContentBySpecifications(ContentSpecification specification);
        CmsContent Create(CmsContent content);
        CmsContent Update(CmsContent content);
        void Delete(CmsContent content);

    }
}
