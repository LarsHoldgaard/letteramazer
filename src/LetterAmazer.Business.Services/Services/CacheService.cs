using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void DeleteByContaining(string containing)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<string> deleteList = new List<string>();
            HttpContext oc = HttpContext.Current;

            // find all cache keys in the system... maybe insane? I don't know lol
            IDictionaryEnumerator en = oc.Cache.GetEnumerator();
            while (en.MoveNext())
            {
                var k = en.Key.ToString();
                if (k.Contains(containing))
                {
                    deleteList.Add(k);
                }
            }

            foreach (var del in deleteList)
            {
                Delete(del);   
            }


            watch.Stop();
            logger.Info(string.Format("DeleteByContaining containing {0} took {1} ms",containing,watch.ElapsedMilliseconds));
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
