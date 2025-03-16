using Microsoft.EntityFrameworkCore;

using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Library
{
    public class MongoGenericRepository<T> where T : class
    {
        DbContext _context;

        public MongoGenericRepository(DbContext context)
        {
            _context = context;
            _context.Database.AutoTransactionBehavior = Microsoft.EntityFrameworkCore.AutoTransactionBehavior.Never;
        }

        public async Task<T> AddAsync(T entity)
        {
            var idProperty = entity.GetType().GetProperty("id");
            if (idProperty != null)
            {
                // Set the MongoDB ObjectId as the unique identifier
                idProperty.SetValue(entity, ObjectId.GenerateNewId().ToString());
            }

            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        //Add multiple entities and return the added entities
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("The entities collection is null or empty.");

            foreach (var entity in entities)
            {
                var idProperty = entity.GetType().GetProperty("id");
                if (idProperty != null)
                {
                    // Set the MongoDB ObjectId as the unique identifier
                    idProperty.SetValue(entity, ObjectId.GenerateNewId().ToString());
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

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_context.Set<T>().AsEnumerable());
        }

        public Task<T> GetByIdAsync(int id)
        {
            return Task.FromResult(_context.Set<T>().Find(id))!;
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
