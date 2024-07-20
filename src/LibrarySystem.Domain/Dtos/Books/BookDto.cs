using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Dtos.Reviews;

namespace LibrarySystem.Domain.Dtos.Books;

public record class BookDto
{
    public Guid Id { get; set; }
    public string? ISBN { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int PagesCount { get; set; } 
    public DateTimeOffset PublishedDate { get; set; }
    public bool IsAvailable { get; set; }
    public List<byte[]>? CoverPictures { get; set; }
    public List<AuthorDto> Authors { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
    public List<BookReviewDto> Reviews { get; set; } = [];
}
