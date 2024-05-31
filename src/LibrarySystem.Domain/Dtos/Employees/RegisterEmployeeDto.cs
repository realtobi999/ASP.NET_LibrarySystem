using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Employees;

public record class RegisterEmployeeDto
{
    public Guid? Id { get; set; }
    
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
