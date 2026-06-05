namespace MyFirstApi.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public required string Email { get; set; }   
        public required string Password { get; set; } 
    }
}
