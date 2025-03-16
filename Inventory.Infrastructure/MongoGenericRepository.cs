using Domain.Contracts;

using Inventory.Domain.Contracts;
using Inventory.Infrastructure.Contexts;

using Microsoft.EntityFrameworkCore;

using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MongoGenericRepository<T> : IGenericRepository<T> where T : class
    {
        MongoDbContext _context;

        public MongoGenericRepository(MongoDbContext context)
        {
            _context = context;
            _context.Database.AutoTransactionBehavior = Microsoft.EntityFrameworkCore.AutoTransactionBehavior.Never;
        }

        public IQueryable<T> Table => _context.Set<T>().AsNoTracking().AsQueryable();

        public async Task<T> AddAsync(T entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                // Set the MongoDB ObjectId as the unique identifier
                idProperty.SetValue(entity, ObjectId.GenerateNewId().GetHashCode());
            }

            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        //Add multiple entities and return the added entities
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("The entities collection is null or empty.");

            PropertyInfo? idProperty = null;

            foreach (var entity in entities)
            {

                idProperty = entity.GetType().GetProperty("Id");
                if (idProperty != null)
                {
                    // Set the MongoDB ObjectId as the unique identifier
                    idProperty.SetValue(entity, ObjectId.GenerateNewId().GetHashCode());
                }
            }

            await _context.Set<T>().AddRangeAsync(entities);

            return entities;
        }

        public T DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }

        //Delete multiple entities and return the deleted entities
        public IEnumerable<T> DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return entities;
        }

        public IQueryable<T> Find(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }

        public Task<T?> FirstAsync()
        {
            return Task.FromResult(_context.Set<T>().FirstOrDefault());
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_context.Set<T>().AsEnumerable());
        }

        public Task<T> GetByIdAsync(int id)
        {
            return Task.FromResult(_context.Set<T>().Find(id))!;
        }

        public Task<T?> LastAsync()
        {
            return Task.FromResult(_context.Set<T>().LastOrDefault());
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            return Task.FromResult(entities);
        }
    }
}
