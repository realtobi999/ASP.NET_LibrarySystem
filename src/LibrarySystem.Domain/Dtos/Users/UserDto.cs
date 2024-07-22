using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public Picture? ProfilePicture { get; set; }
}
