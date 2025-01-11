using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Books;

public record class CreateBookDto
{
    public Guid? Id { get; init; }

    [Required]
    public required string ISBN { get; init; }

    [Required, MaxLength(55)]
    public required string Title { get; init; }

    [Required, MaxLength(555)]
    public required string Description { get; init; }

    [Required, Range(0, 10000)]
    public required int PagesCount { get; init; }

    [Required]
    public required DateTimeOffset PublishedDate { get; init; }

    public bool? Available { get; init; }

    [Required, MaxLength(15)]
    public required IEnumerable<Guid> GenreIds { get; init; } = [];

    [Required, MaxLength(15)]
    public required IEnumerable<Guid> AuthorIds { get; init; } = [];
}
