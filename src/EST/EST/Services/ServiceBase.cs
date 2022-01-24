using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;

namespace EST.Services
{
    public class ServiceBase
    {
        public ServiceBase(IBlobCache cache)
        {
            Cache = cache ?? BlobCache.LocalMachine;
        }

        public async Task<T> GetFromCache<T>(string cacheName)
        {
            try
            {
                T t = await Cache.GetObject<T>(cacheName);
                return t;
            }
            catch(KeyNotFoundException)
            {
                return default(T);
            }
        }

        public void InvalidateCache()
        {
            Cache.InvalidateAll();
        }

        protected IBlobCache Cache;
    }
}
