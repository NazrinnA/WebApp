using Microsoft.EntityFrameworkCore;
using WebApplication2.Entities;

namespace WebApplication2.DataAccess
{
    public class AppDb:DbContext
    {
        public AppDb(DbContextOptions<AppDb> options):base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
