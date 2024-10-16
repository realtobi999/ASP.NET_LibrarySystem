using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Enums;

namespace LibrarySystem.Domain.Entities;

public class Picture
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("book_id")]
    public Guid EntityId { get; set; }

    [Required, Column("entity_type")]
    public PictureEntityType EntityType { get; set; }

    [Required, Column("name")]
    public string? FileName { get; set; }

    [Required, Column("content")]
    public byte[]? FileContent { get; set; }

    [Required, Column("mime_type")]
    public string? MimeType { get; set; }

    // relationships

    [JsonIgnore]
    public Book? Book { get; set; }
    [JsonIgnore]
    public Author? Author { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public Employee? Employee { get; set; }
}
