using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Authors;

public class UpdateAuthorDto
{
    [Required, MaxLength(55)]
    public string? Name { get; set; }

    [Required, MaxLength(1555)]
    public string? Description { get; set; }

    [Required]
    public DateTimeOffset Birthday { get; set; }

    public string? ProfilePicture { get; set; }
}
