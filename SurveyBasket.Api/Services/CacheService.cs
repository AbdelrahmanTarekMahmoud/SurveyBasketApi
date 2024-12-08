
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurveyBasket.Api.Services
{
    public sealed class CacheService(IDistributedCache distributedCache): ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class
        {
            var CachedValue = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
            
            return  string.IsNullOrEmpty(CachedValue) ? null : JsonSerializer.Deserialize<T>(CachedValue);

        }

        public async Task RemoveAsync (string cacheKey, CancellationToken cancellationToken = default) 
        {
             await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
        }

        public async Task SetAsync<T>(string cacheKey, T Value, CancellationToken cancellationToken = default) where T : class
        {
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(Value) , cancellationToken);
        }
    }
}
