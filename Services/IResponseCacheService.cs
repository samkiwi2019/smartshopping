using System;
using System.Threading.Tasks;

namespace Smartshopping.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLiveSeconds);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}