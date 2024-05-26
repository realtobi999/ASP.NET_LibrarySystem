using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginUserResponseDto
{
    public User? User { get; set; }
    public string? Token { get; set; }
}
