using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Genres;

namespace LibrarySystem.Domain.Dtos.Books;

public record class UpdateBookDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int PagesCount { get; set; } 
    public bool? Availability { get; set; }
    public DateTimeOffset PublishedDate { get; set; }
    public string? CoverPicture { get; set; }
    public List<AuthorDto> Authors { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
}
