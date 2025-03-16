using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

using Sales.Domain.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sales.Infrastructure
{
    public class InMemoryCachingService : ICachingService
    {

        private readonly IMemoryCache _cache;

        public InMemoryCachingService(IMemoryCache cache)
        {
            _cache = cache;
        }


        public Task<T?> GetAsync<T>(string cacheKey)
        {
            if (!_cache.TryGetValue(cacheKey, out string? data))
            {
                return Task.FromResult<T?>(default);
            }
            else
            {
                return Task.FromResult<T?>(JsonSerializer.Deserialize<T>(data!));
            }
        }

        public Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, int expirationMinutes = 60)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string cacheKey)
        {
            _cache.Remove(cacheKey);
            return Task.CompletedTask;
        }

        public Task<T> SetAsync<T>(string cacheKey, T item, int expirationMinutes = 60)
        {
            var serializedItem = JsonSerializer.Serialize(item);
            _cache.Set(cacheKey, serializedItem, TimeSpan.FromMinutes(expirationMinutes));
            return Task.FromResult(item);
        }
    }
}
