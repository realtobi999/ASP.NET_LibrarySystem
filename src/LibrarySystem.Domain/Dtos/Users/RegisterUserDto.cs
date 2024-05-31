using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class RegisterUserDto
{
    public Guid? Id { get; set; }
    
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
