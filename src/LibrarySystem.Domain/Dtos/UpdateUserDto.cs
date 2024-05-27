using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain;

public class UpdateUserDto
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Email { get; set; }
}
