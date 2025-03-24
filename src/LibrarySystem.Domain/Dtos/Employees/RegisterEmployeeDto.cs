using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public record RegisterEmployeeDto
{
    public Guid? Id { get; init; }

    [Required, MaxLength(55)]
    public required string Name { get; init; }

    [Required, EmailAddress, MaxLength(155)]
    public required string Email { get; init; }

    [Required, MinLength(8), MaxLength(55)]
    public required string Password { get; init; }
}