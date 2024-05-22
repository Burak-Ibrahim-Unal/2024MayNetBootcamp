using Bootcamp.Clean.ApplicationService.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.Cache.InMemoryCache
{
    public class InMemoryCacheService(IMemoryCache _memoryCache) : ICacheService
    {
        public void Add<T>(string key, T value)
        {
            _memoryCache.Set(key, value);
        }

        public T? Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
