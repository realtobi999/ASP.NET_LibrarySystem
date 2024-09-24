using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Genres;

public record class CreateGenreDto
{
    public Guid? Id { get; set; }

    [Required, MaxLength(55)]
    public string? Name { get; set; }
}
