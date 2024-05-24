using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Entities;

public class Genre
{
    [Required, Column("id")]
    public Guid Id { get; set; }
    
    [Required, Column("name")]
    public string? Name { get; set; }
}
