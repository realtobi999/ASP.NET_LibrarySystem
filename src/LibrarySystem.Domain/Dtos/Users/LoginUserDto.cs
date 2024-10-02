using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class LoginUserDto
{
    [Required]
    public string? Email { get; init; }

    [Required]
    public string? Password { get; init; }
}
