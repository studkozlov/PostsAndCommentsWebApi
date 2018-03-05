using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestApp.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> pred);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
    }
}
