using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Entities.Relationships;

namespace LibrarySystem.Domain.Entities;

public class Book
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("isbn")]
    public string? ISBN { get; set; }

    [Required, Column("title")]
    public string? Title { get; set; }

    [Required, Column("description")]
    public string? Description { get; set; }

    [Required, Column("pages_count")]
    public int PagesCount { get; set; } 

    [Required, Column("published_at")]
    public DateTimeOffset PublishedAt { get; set; }

    [Required, Column("cover_photo")]
    public byte[]? CoverPicture { get; set; }

    // relationship

    public ICollection<BookAuthor> BookAuthors { get; set; } = [];
    public ICollection<BookGenre> BookGenres { get; set; } = [];
}
