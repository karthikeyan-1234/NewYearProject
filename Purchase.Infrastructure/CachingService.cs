using Microsoft.Extensions.Caching.Distributed;

using Purchases.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Purchases.Infrastructure
{
    public class CachingService : ICachingService
    {
        IDistributedCache _cache;

        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string cacheKey)
        {
            var result = await _cache.GetStringAsync(cacheKey);
            if (result == null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, int expirationMinutes = 60)
        {
            // Check if the cache key exists
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (cachedData != null)
            {
                return JsonSerializer.Deserialize<T>(cachedData)!;
            }

            // If the cache key does not exist, call the getItemCallback function to get the data
            var item = await getItemCallback();
            if (item != null)
            {
                // Serialize the data and store it in the cache
                var serializedItem = JsonSerializer.Serialize(item);
                await _cache.SetStringAsync(cacheKey, serializedItem, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
                });
            }
            else
            {
                // If the getItemCallback function returns null, store an empty string in the cache
                await _cache.SetStringAsync(cacheKey, "", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
                });
            }
            return item;
         }

        public async Task RemoveAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task<T> SetAsync<T>(string cacheKey, T item, int expirationMinutes = 60)
        {
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(item), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
            });

            return item;
        }
    }
}
