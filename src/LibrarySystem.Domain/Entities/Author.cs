using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Author : IDtoSerialization<AuthorDto>
{
    // core properties

    [Required, Column("id")]
    public required Guid Id { get; set; }

    [Required, Column("name")]
    public required string Name { get; set; }

    [Required, Column("description")]
    public required string Description { get; set; }

    [Required, Column("birthday")]
    public required DateTimeOffset Birthday { get; set; }

    [Required, Column("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    // relationships

    [JsonIgnore]
    public virtual Picture? Picture { get; set; }

    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = [];

    /// <inheritdoc/>
    public AuthorDto ToDto()
    {
        return new AuthorDto
        {
            Id = this.Id,
            Name = this.Name,
            Description = this.Description,
            Birthday = this.Birthday,
            Picture = this.Picture,
        };
    }

    public void Update(UpdateAuthorDto dto)
    {
        Name = dto.Name;
        Description = dto.Description;
        Birthday = dto.Birthday;
    }
}