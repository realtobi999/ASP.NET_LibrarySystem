using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Author : IDtoSerializable<AuthorDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("name")]
    public string? Name { get; set; }

    [Required, Column("description")]
    public string? Description { get; set; }

    [Required, Column("birthday")]
    public DateTimeOffset Birthday { get; set; }

    // relationships
    
    [JsonIgnore]
    public Picture? Picture { get; set; }

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
}
