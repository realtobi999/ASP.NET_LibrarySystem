using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Books;

public record class BookDto
{
    public required Guid Id { get; init; }
    public required string? ISBN { get; init; }
    public required string? Title { get; init; }
    public required string? Description { get; init; }
    public required int PagesCount { get; init; }
    public required DateTimeOffset PublishedDate { get; init; }
    public required bool IsAvailable { get; init; }
    public required List<Picture>? CoverPictures { get; init; }
    public required List<AuthorDto> Authors { get; init; } = [];
    public required List<GenreDto> Genres { get; init; } = [];
    public required List<BookReviewDto> Reviews { get; init; } = [];
}
