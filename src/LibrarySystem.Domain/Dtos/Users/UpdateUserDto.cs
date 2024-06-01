using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UpdateUserDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
}
