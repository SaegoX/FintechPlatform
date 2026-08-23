using LoanService.Application.Common;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace LoanService.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            var cachedString = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(cachedString))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedString);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var jsonOptions = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                jsonOptions.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            var jsonString = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, jsonString, jsonOptions, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}