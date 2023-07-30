using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTO.Author
{
    public class AuthorUpdateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
    }
}
