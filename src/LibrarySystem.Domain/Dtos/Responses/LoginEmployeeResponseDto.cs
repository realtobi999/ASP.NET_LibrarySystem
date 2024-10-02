using LibrarySystem.Domain.Dtos.Employees;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginEmployeeResponseDto
{
    public EmployeeDto? EmployeeDto { get; init; }
    public string? Token { get; init; }
}
