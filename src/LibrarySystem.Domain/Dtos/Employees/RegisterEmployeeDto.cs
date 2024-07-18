using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public record class RegisterEmployeeDto
{
    public Guid? Id { get; set; }
    
    [Required, MaxLength(55)]
    public string? Name { get; set; }

    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; set; }

    [Required, MinLength(8), MaxLength(55)]
    public string? Password { get; set; }
}
