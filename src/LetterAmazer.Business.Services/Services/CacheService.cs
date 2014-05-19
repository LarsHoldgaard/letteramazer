using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LetterAmazer.Business.Services.Domain.Caching;
using System.Web;
using System.Web.Caching;
using log4net;

namespace LetterAmazer.Business.Services.Services
{
    public class CacheService:ICacheService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CacheService));


        public object GetById(string cacheKey)
        {
            if (ContainsKey(cacheKey))
            {
                logger.Info("Cache key retrieved: " + cacheKey);
                return HttpContext.Current.Cache[cacheKey];
            }

            logger.Info("Cache key missed: " + cacheKey);
            return null;
        }

        public object Create(string cacheKey, object obj)
        {
            return Create(cacheKey, obj, DateTime.UtcNow.AddHours(5));
        }

        public object Create(string cacheKey, object obj, DateTime expire)
        {
            logger.Info("Cache key inserted: " + cacheKey);
            if (!ContainsKey(cacheKey))
            {
                HttpContext.Current.Cache.Insert(cacheKey,
                    obj,
                    null,
                    expire,
                    Cache.NoSlidingExpiration);
            }
            return GetById(cacheKey);
        }

        public void Delete(string cacheKey)
        {
            HttpContext.Current.Cache.Remove(cacheKey);
        }

        
        public bool ContainsKey(string cacheKey)
        {
            return HttpContext.Current.Cache[cacheKey] != null;
        }

        public string GetCacheKey(string methodName, string value)
        {
            return string.Format("{0}_{1}", methodName, value);
        }
    }
}
