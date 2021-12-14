using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCars.Abstraction
{
    public interface IUnitOfWork
    {
        IQueryable<T> Query<T>() where T : class;

        void Add<T>(T entity) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;

        void Remove<T>(T entity) where T : class;
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;

        Task CommitAsync();
    }
}
