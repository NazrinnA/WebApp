using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using System.Linq.Expressions;
using WebApplication2.DataAccess;
using WebApplication2.Entities;
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

        public async Task Create(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await Save();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<T> Get(Expression<Func<T, bool>> exp = null, params string[] includes)
        {
            IQueryable<T> entity =  _db.Set<T>();
            if(includes is not null)
            {
                foreach(var include in includes)
                {
                    entity=entity.Include(include);
                }
            }
            if (entity is null) throw new Exception();
            return exp==null ?await entity.FirstOrDefaultAsync():await entity.Where(exp).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> exp = null, params string[] includes)
        {
            IQueryable<T> entity = _db.Set<T>();
            if (entity is null) throw new Exception();
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }
            }
            return exp == null ? await entity.ToListAsync() : await entity.Where(exp).ToListAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _db.Set<T>().Update(entity);
            await Save();
        }
    }
}
