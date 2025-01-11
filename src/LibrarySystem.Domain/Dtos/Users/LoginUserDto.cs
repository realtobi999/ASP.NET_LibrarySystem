using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class LoginUserDto
{
    [Required]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}
