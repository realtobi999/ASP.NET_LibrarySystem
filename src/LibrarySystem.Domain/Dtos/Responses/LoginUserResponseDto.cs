using LibrarySystem.Domain.Dtos.Users;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginUserResponseDto
{
    public required UserDto UserDto { get; init; }
    public required string Token { get; init; }
}
