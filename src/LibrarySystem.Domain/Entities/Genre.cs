using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Dtos.Genres;

namespace LibrarySystem.Domain.Entities;

public class Genre
{
    [Required, Column("id")]
    public Guid Id { get; set; }
    
    [Required, Column("name")]
    public string? Name { get; set; }

    public GenreDto ToDto()
    {
        return new GenreDto
        {
            Id = this.Id,
            Name = this.Name
        };
    }
}
