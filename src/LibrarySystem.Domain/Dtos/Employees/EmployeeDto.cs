using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Employees;

public record class EmployeeDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public Picture? Picture { get; set; }
}