using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Dtos.Authors;

namespace LibrarySystem.Domain.Entities;

public class Author
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("name")]
    public string? Name { get; set; }

    [Required, Column("description")]
    public string? Description { get; set; }

    [Required, Column("birthday")]
    public DateTimeOffset Birthday { get; set; }

    [Column("profile_photo")]
    public byte[]? ProfilePicture { get; set; }

    public AuthorDto ToDto()
    {
        return new AuthorDto
        {
            Id = this.Id,
            Name = this.Name,
            Description = this.Description,
            Birthday = this.Birthday,
            ProfilePicture = Convert.ToBase64String(this.ProfilePicture ?? [])
        };
    }
}
