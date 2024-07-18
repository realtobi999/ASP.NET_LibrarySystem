using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public class UpdateEmployeeDto
{
    [Required, MaxLength(55)]
    public string? Name { get; set; }

    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; set; }
}
