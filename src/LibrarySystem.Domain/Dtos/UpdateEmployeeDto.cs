using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos;

public class UpdateEmployeeDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }
}
