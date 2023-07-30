using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTO.Book
{
    public class BookCreateDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public List<int> AuthorsId { get; set; }
    }
}
