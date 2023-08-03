using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication2.Entities;

namespace WebApplication2.Repository.Interfaces
{
    public interface IRepository<T> : IDisposable 
    {
        Task Create(T entity);
        Task<T> Get(Expression<Func<T, bool>> exp = null, params string[] includes);
        Task<List<T>> GetAll(Expression<Func<T, bool>> exp = null, params string[] includes);
        Task Update(T entity);
        Task Save();
    }
}
