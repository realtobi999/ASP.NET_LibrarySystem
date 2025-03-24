using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Genres;

public record UpdateGenreDto
{
    [Required, MaxLength(55)]
    public required string Name { get; init; }
}