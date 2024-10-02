using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Genres;

public record class CreateGenreDto
{
    public Guid? Id { get; init; }

    [Required, MaxLength(55)]
    public string? Name { get; init; }
}
