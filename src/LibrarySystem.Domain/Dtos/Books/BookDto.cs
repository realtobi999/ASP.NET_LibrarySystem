using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Books;

public record class BookDto
{
    public Guid Id { get; init; }
    public string? ISBN { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int PagesCount { get; init; }
    public DateTimeOffset PublishedDate { get; init; }
    public bool IsAvailable { get; init; }
    public List<Picture>? CoverPictures { get; init; }
    public List<AuthorDto> Authors { get; init; } = [];
    public List<GenreDto> Genres { get; init; } = [];
    public List<BookReviewDto> Reviews { get; init; } = [];
}
