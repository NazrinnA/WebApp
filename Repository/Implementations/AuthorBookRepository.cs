using WebApplication2.DataAccess;
using WebApplication2.Entities;
using WebApplication2.Repository.Interfaces;

namespace WebApplication2.Repository.Implementations
{
    public class AuthorBookRepository : Repository<AuthorBook>, IAuthorBookRepository
    {
        public AuthorBookRepository(AppDb db) : base(db)
        {
        }
    }
}
