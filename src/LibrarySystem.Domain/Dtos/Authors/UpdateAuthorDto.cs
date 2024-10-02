using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Authors;

public class UpdateAuthorDto
{
    [Required, MaxLength(55)]
    public string? Name { get; init; }

    [Required, MaxLength(1555)]
    public string? Description { get; init; }

    [Required]
    public DateTimeOffset Birthday { get; init; }
}
