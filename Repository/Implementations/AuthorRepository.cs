using WebApplication2.DataAccess;
using WebApplication2.Entities;
using WebApplication2.Repository.Interfaces;

namespace WebApplication2.Repository.Implementations
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDb db) : base(db)
        {
        }
    }
}
