using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApplication2.DataAccess;
using WebApplication2.Repository.Interfaces;

namespace WebApplication2.Repository.Implementations
{
    public class Repository<T> : IRepository<T>
        where T : class, new()
    {

        private readonly AppDb _db;
        public Repository(AppDb db)
        {
            _db = db;
        }

        public async Task  Create(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await Save();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<T> Get(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            if (entity is null) throw new Exception();
            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _db.Set<T>().Update(entity);
            Save();
        }
    }
}
