using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using LetterAmazer.Business.Services.Domain.Caching;

namespace LetterAmazer.Business.Services.Services.Caching
{
    public class MemoryCache:ICacheService
    {
        public void Clear()
        {
            System.Runtime.Caching.MemoryCache.Default.Dispose();
        }

        public object GetById(string cacheKey)
        {
            if (ContainsKey(cacheKey))
            {
                return System.Runtime.Caching.MemoryCache.Default[cacheKey];
            }

            return null;
        }

        public object Create(string cacheKey, object obj)
        {
            return Create(cacheKey, obj, DateTime.UtcNow.AddHours(5));
        }

        public object Create(string cacheKey, object obj, DateTime expire)
        {
            if (!ContainsKey(cacheKey))
            {
                System.Runtime.Caching.MemoryCache.Default.Add(cacheKey,
                    obj,
                    new CacheItemPolicy() {});
            }
            return GetById(cacheKey);
        }

        public void Delete(string cacheKey)
        {
            System.Runtime.Caching.MemoryCache.Default.Remove(cacheKey);
        }

        public void DeleteByContaining(string containing)
        {
            // yeah, we are not going to do that
        }


        public bool ContainsKey(string cacheKey)
        {
            return System.Runtime.Caching.MemoryCache.Default[cacheKey] != null;
        }

        public string GetCacheKey(string methodName, string value)
        {
            return string.Format("{0}_{1}", methodName, value);
        }
    }
}
