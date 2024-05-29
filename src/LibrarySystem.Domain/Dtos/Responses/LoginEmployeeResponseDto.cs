using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginEmployeeResponseDto
{
    public EmployeeDto? EmployeeDto { get; set; }
    public string? Token { get; set; }
}
