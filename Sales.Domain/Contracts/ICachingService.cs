using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Domain.Contracts
{
    public interface ICachingService
    {
        Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, int expirationMinutes = 60);
        Task RemoveAsync(string cacheKey);
        Task<T?> GetAsync<T>(string cacheKey);
        Task<T> SetAsync<T>(string cacheKey, T item, int expirationMinutes = 60);
    }
}
