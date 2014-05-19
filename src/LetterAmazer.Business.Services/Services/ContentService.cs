using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;
using LetterAmazer.Business.Services.Domain.Caching;
using LetterAmazer.Business.Services.Domain.Content;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Business.Services.Services
{
    public class ContentService:IContentService
    {
        private LetterAmazerEntities repository;
        private IContentFactory contentFactory;
        private ICacheService cacheService;

        public ContentService(LetterAmazerEntities letterAmazerEntities, IContentFactory contentFactory,ICacheService cacheService)
        {
            this.repository = letterAmazerEntities;
            this.contentFactory = contentFactory;
            this.cacheService = cacheService;
        }

        public CmsContent GetContentById(int id)
        {
             var cacheKey = cacheService.GetCacheKey(MethodBase.GetCurrentMethod().Name, id.ToString());
            if (!cacheService.ContainsKey(cacheKey))
            {
                var dbContent = repository.DbCmsContent.FirstOrDefault(c => c.Id == id);

                if (dbContent == null)
                {
                    throw new ArgumentException("No content with this ID");
                }

                var content = contentFactory.Create(dbContent);
                cacheService.Create(cacheKey, content);
                return content;
            }
            return (CmsContent) (cacheService.GetById(cacheKey));

        }

        public List<CmsContent> GetContentBySpecifications(ContentSpecification specification)
        {
            IQueryable<DbCmsContent> dbContent = repository.DbCmsContent;

            if (!string.IsNullOrEmpty(specification.Alias))
            {
                specification.Alias = specification.Alias.ToLower();
                dbContent = dbContent.Where(c => c.Alias == specification.Alias);
            }
            if (!string.IsNullOrEmpty(specification.Section))
            {
                specification.Section = specification.Section.ToLower();
                dbContent = dbContent.Where(c => c.Section == specification.Section);
            }

            return
                contentFactory.Create(
                    dbContent.OrderBy(c => c.Id)
                        .Where(c => c.Enabled)
                        .Skip(specification.Skip)
                        .Take(specification.Take)
                        .ToList());
        }

        public CmsContent Create(CmsContent content)
        {
            var dbContent = new DbCmsContent();
            dbContent.Enabled = content.Enabled;
            dbContent.SeoTitle = content.SeoTitle;
            dbContent.Headline = content.Headline;
            dbContent.Description = content.Content;
            dbContent.MetaDescription = content.MetaDescription;
            dbContent.DateCreated = DateTime.Now;
            dbContent.CmsKey = content.CmsKey;
            dbContent.Section = content.Section;
            dbContent.Alias = content.Alias;

            repository.DbCmsContent.Add(dbContent);
            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetContentById",dbContent.Id.ToString()));
            return GetContentById(dbContent.Id);
        }

        public CmsContent Update(CmsContent content)
        {
            var dbContent = repository.DbCmsContent.FirstOrDefault(c => c.Id == content.Id);

            if (dbContent == null)
            {
                throw new ArgumentException("No content with this ID");
            }

            dbContent.Enabled = content.Enabled;
            dbContent.SeoTitle = content.SeoTitle;
            dbContent.Headline = content.Headline;
            dbContent.Description = content.Content;
            dbContent.MetaDescription = content.MetaDescription;
            dbContent.DateCreated = DateTime.Now;
            dbContent.CmsKey = content.CmsKey;
            dbContent.Section = content.Section;
            dbContent.Alias = content.Alias;

            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetContentById", dbContent.Id.ToString()));
            return GetContentById(content.Id);
        }

        public void Delete(CmsContent content)
        {
            var dbContent = repository.DbCmsContent.FirstOrDefault(c => c.Id == content.Id);

            if (dbContent == null)
            {
                throw new ArgumentException("No content with this ID");
            }

            repository.DbCmsContent.Remove(dbContent);
            repository.SaveChanges();

            cacheService.Delete(cacheService.GetCacheKey("GetContentById", dbContent.Id.ToString()));
        }


    }
}
