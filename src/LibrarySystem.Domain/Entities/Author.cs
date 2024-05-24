using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Entities;

public class Author
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("name")]
    public string? Name { get; set; }

    [Required, Column("description")]
    public string? Description { get; set; }

    [Required, Column("profile_photo")]
    public byte[]? ProfilePicture { get; set; }
}
