using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Entities;

public class Picture
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("book_id")]
    public Guid BookId { get; set; }

    [Required, Column("name")]
    public string? FileName { get; set; }

    [Required, Column("content")]
    public byte[]? FileContent { get; set; }

    [Required, Column("mime_type")]
    public string? MimeType { get; set; }

    // relationships
    
    public Book? Book { get; set; }
}
