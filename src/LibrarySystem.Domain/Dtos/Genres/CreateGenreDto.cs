using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Dtos.Genres;

public record class CreateGenreDto
{
    public Guid? Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
}
