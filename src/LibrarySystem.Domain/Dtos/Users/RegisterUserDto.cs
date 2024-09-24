using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class RegisterUserDto
{
    public Guid? Id { get; set; }

    [Required, MaxLength(55)]
    public string? Username { get; set; }

    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; set; }

    [Required, MinLength(8), MaxLength(55)]
    public string? Password { get; set; }
}
