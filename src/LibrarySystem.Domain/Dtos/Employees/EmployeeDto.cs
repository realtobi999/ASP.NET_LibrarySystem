using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Employees;

public record EmployeeDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required Picture? Picture { get; init; }
}