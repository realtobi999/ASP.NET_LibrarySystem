using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Books;

public record class CreateBookDto
{
    public Guid? Id { get; set; }

    [Required]
    public string? ISBN { get; set; }

    [Required, MaxLength(55)]
    public string? Title { get; set; }

    [Required, MaxLength(555)]
    public string? Description { get; set; }

    [Required, Range(0,10000)]
    public int PagesCount { get; set; } 

    [Required]
    public DateTimeOffset PublishedDate { get; set; }

    [Required]
    public bool? Available { get; set; }

    [Required, MaxLength(15)]
    public IEnumerable<Guid>? GenreIds { get; set; }

    [Required, MaxLength(15)]
    public IEnumerable<Guid>? AuthorIds { get; set; }
}
