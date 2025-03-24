using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Genre : IDtoSerialization<GenreDto>
{
    // core properties

    [Required, Column("id")]
    public required Guid Id { get; init; }

    [Required, Column("name")]
    public required string Name { get; set; }

    [Required]
    public required DateTimeOffset CreatedAt { get; init; }

    // relationships

    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = [];

    /// <inheritdoc/>
    public GenreDto ToDto()
    {
        return new GenreDto
        {
            Id = this.Id,
            Name = this.Name
        };
    }

    public void Update(UpdateGenreDto dto)
    {
        Name = dto.Name;
    }
}