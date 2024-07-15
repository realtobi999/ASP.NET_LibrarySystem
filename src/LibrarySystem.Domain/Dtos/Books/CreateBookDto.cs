using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Books;

public record class CreateBookDto
{
    public Guid? Id { get; set; }

    [Required]
    public string? ISBN { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public int PagesCount { get; set; } 

    [Required]
    public DateTimeOffset PublishedDate { get; set; }

    [Required]
    public bool? Available { get; set; }

    [Required]
    public IEnumerable<Guid>? GenreIds { get; set; }

    [Required]
    public IEnumerable<Guid>? AuthorIds { get; set; }
    
    public string? CoverPicture { get; set; }
}
