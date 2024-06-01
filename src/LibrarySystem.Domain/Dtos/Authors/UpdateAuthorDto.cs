using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain;

public class UpdateAuthorDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public DateTimeOffset Birthday { get; set; }

    [Required]
    public string? ProfilePicture { get; set; }
}
