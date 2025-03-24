using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Books;

public record UpdateBookDto
{
    [Required, MaxLength(55)]
    public required string Title { get; init; }

    [Required, MaxLength(555)]
    public required string Description { get; init; }

    [Required, Range(0, 10000)]
    public required int PagesCount { get; init; }

    [Required]
    public bool? Availability { get; init; }

    [Required]
    public required DateTimeOffset PublishedDate { get; init; }

    [Required, MaxLength(15)]
    public required IEnumerable<Guid> GenreIds { get; init; } = [];

    [Required, MaxLength(15)]
    public required IEnumerable<Guid> AuthorIds { get; init; } = [];
}