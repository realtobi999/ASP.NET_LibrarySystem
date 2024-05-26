using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginStaffResponseDto
{
    public Staff? Staff { get; set; }
    public string? Token { get; set; }
}
