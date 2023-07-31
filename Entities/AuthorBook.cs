using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Entities
{
    public class AuthorBook
    {
        public int Id { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }

    }
}
