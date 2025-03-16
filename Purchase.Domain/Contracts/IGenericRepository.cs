using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateEntry(T oldEntity, T entity);
        T DeleteAsync(T entity);
        IEnumerable<T> DeleteRange(IEnumerable<T> entities);
        Task SaveChangesAsync();
        IQueryable<T> Find(Func<T, bool> predicate);

        IQueryable<T> Table { get; }

        Task<T?> FirstAsync();
        Task<T?> LastAsync();
    }
}
