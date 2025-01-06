using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Authors;

public class UpdateAuthorDto
{
    [Required, MaxLength(55)]
    public required string Name { get; init; }

    [Required, MaxLength(1555)]
    public required string Description { get; init; }

    [Required]
    public required DateTimeOffset Birthday { get; init; }
}
