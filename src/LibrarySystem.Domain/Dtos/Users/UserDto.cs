using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UserDto
{
    public Guid Id { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
    public Picture? ProfilePicture { get; init; }
}
