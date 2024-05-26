namespace LibrarySystem.Domain.Dtos;

public record class UserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
}
