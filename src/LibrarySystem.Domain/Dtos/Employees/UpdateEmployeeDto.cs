using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public class UpdateEmployeeDto
{
    [Required, MaxLength(55)]
    public required string Name { get; init; }

    [Required, EmailAddress, MaxLength(155)]
    public required string Email { get; init; }
}
