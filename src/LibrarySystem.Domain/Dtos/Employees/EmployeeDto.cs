using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Employees;

public record class EmployeeDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public Picture? Picture { get; init; }
}