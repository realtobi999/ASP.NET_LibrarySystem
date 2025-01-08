using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UpdateUserDto
{
    [Required, MaxLength(55)]
    public required string Username { get; init; }

    [Required, EmailAddress, MaxLength(155)]
    public required string Email { get; init; }
}
