using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public record class RegisterEmployeeDto
{
    public Guid? Id { get; init; }

    [Required, MaxLength(55)]
    public string? Name { get; init; }

    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; init; }

    [Required, MinLength(8), MaxLength(55)]
    public string? Password { get; init; }
}
