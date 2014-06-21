using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Services.Domain.Caching
{
    public interface ICacheService
    {
        void Clear();
        object GetById(string cacheKey);
        object Create(string cacheKey, object obj);
        object Create(string cacheKey, object obj,DateTime expire);
        void Delete(string cacheKey);
        void DeleteByContaining(string containing);
        string GetCacheKey(string methodName, string value);
        bool ContainsKey(string cacheKey);
    }
}
