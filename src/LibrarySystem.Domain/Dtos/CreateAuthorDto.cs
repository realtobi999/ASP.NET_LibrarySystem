using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos;

public class CreateAuthorDto
{
    public Guid? Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public DateTimeOffset Birthday { get; set; }

    public string? ProfilePicture { get; set; }
}
