using LibrarySystem.Domain.Dtos.Users;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginUserResponseDto
{
    public UserDto? UserDto { get; init; }
    public string? Token { get; init; }
}
