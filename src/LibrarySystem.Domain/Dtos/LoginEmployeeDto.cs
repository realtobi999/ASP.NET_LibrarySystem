using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain;

public record class LoginEmployeeDto
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
