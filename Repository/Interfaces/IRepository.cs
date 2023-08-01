using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Repository.Interfaces
{
    public interface IRepository<T> : IDisposable 
    {
        Task Create(T entity);
        Task<T> Get(int id);
        Task<List<T>> GetAll();
        Task Update(T entity);
        Task Save();
    }
}
