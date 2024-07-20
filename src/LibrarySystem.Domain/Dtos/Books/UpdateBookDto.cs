using System.ComponentModel.DataAnnotations;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Genres;

namespace LibrarySystem.Domain.Dtos.Books;

public record class UpdateBookDto
{
    [Required, MaxLength(55)]
    public string? Title { get; set; }

    [Required, MaxLength(555)]
    public string? Description { get; set; }

    [Required, Range(0,10000)]
    public int PagesCount { get; set; } 

    [Required]
    public bool? Availability { get; set; }

    [Required]
    public DateTimeOffset PublishedDate { get; set; }

    public List<AuthorDto>? Authors { get; set; }
    public List<GenreDto>? Genres { get; set; } 
}
