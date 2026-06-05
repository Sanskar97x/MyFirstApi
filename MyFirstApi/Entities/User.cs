using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Entities
{
    public class User
    {
        [Key] 
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
