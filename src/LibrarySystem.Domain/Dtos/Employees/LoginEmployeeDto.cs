using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public record LoginEmployeeDto
{
    [Required]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}