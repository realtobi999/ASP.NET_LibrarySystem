using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos;

public record class LoginUserResponseDto
{
    public User? User { get; set; }
    public string? Token { get; set; }
}
