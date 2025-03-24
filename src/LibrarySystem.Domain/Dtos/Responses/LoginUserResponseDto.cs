using LibrarySystem.Domain.Dtos.Users;

namespace LibrarySystem.Domain.Dtos.Responses;

public record LoginUserResponseDto
{
    public required UserDto UserDto { get; init; }
    public required string Token { get; init; }
}