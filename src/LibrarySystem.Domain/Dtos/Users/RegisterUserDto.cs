using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class RegisterUserDto
{
    public Guid? Id { get; init; }

    [Required, MaxLength(55)]
    public required string Username { get; init; }

    [Required, EmailAddress, MaxLength(155)]
    public required string Email { get; init; }

    [Required, MinLength(8), MaxLength(55)]
    public required string Password { get; init; }

    // TODO: add confirm password validation with attribute
}

