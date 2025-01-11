using LibrarySystem.Domain.Dtos.Employees;

namespace LibrarySystem.Domain.Dtos.Responses;

public record class LoginEmployeeResponseDto
{
    public required EmployeeDto EmployeeDto { get; init; }
    public required string Token { get; init; }
}
