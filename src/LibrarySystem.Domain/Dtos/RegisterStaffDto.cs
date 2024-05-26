using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos;

public record class RegisterStaffDto
{
    public Guid? Id { get; set; }
    
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
