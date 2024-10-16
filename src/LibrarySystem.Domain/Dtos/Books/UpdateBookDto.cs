using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Books;

public record class UpdateBookDto
{
    [Required, MaxLength(55)]
    public string? Title { get; init; }

    [Required, MaxLength(555)]
    public string? Description { get; init; }

    [Required, Range(0, 10000)]
    public int PagesCount { get; init; }

    [Required]
    public bool? Availability { get; init; }

    [Required]
    public DateTimeOffset PublishedDate { get; init; }

    [Required, MaxLength(15)]
    public IEnumerable<Guid> GenreIds { get; init; } = [];

    [Required, MaxLength(15)]
    public IEnumerable<Guid> AuthorIds { get; init; } = [];
}
