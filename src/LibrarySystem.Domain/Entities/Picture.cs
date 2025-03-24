using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Enums;

namespace LibrarySystem.Domain.Entities;

public class Picture
{
    // core properties

    [Required, Column("id")]
    public Guid Id { get; init; }

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

    [Required, Column("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    // relationships

    [JsonIgnore]
    public virtual Book? Book { get; set; }

    [JsonIgnore]
    public virtual Author? Author { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual Employee? Employee { get; set; }
}