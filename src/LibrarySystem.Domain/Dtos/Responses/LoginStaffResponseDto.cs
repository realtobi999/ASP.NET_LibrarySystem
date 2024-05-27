using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginStaffResponseDto
{
    public StaffDto? StaffDto { get; set; }
    public string? Token { get; set; }
}
