using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class LoginUserDto
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
