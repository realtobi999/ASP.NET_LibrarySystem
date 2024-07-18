using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Genres;

public record class UpdateGenreDto
{
    [Required, MaxLength(55)]
    public string? Name { get; set; }
}
