using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Books;

public record class CreateBookDto
{
    public Guid? Id { get; init; }

    [Required]
    public string? ISBN { get; init; }

    [Required, MaxLength(55)]
    public string? Title { get; init; }

    [Required, MaxLength(555)]
    public string? Description { get; init; }

    [Required, Range(0, 10000)]
    public int PagesCount { get; init; }

    [Required]
    public DateTimeOffset PublishedDate { get; init; }

    [Required]
    public bool? Available { get; init; }

    [Required, MaxLength(15)]
    public IEnumerable<Guid> GenreIds { get; init; } = [];

    [Required, MaxLength(15)]
    public IEnumerable<Guid> AuthorIds { get; init; } = [];
}
