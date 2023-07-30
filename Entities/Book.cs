using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        public List<Author> Authors { get; set; }=new List<Author>();
    }
}
