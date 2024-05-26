using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Response;

public record class LoginStaffResponseDto
{
    public Staff? Staff { get; set; }
    public string? Token { get; set; }
}
