using LibrarySystem.Domain.Dtos.Users;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginUserResponseDto
{
    public UserDto? UserDto { get; set; }
    public string? Token { get; set; }
}
