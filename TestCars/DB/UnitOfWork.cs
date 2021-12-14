using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCars.Abstraction;

namespace TestCars.DB
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _db;

        public UnitOfWork(DbContext db)
        {
            _db = db;
        }

        public IQueryable<T> Query<T>() where T : class
            => _db.Set<T>();

        public void Add<T>(T entity) where T : class
            => _db.Set<T>().Add(entity);

        public Task AddAsync<T>(T entity) where T : class
            => _db.Set<T>().AddAsync(entity).AsTask();

        public Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
            => _db.Set<T>().AddRangeAsync(entities);

        public void Remove<T>(T entity) where T : class
            => _db.Set<T>().Remove(entity);

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
            => _db.Set<T>().RemoveRange(entities);

        public async Task CommitAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
