using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UpdateUserDto
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Email { get; set; }
}
