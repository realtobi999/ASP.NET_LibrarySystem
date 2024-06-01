using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain;

public class UpdateAuthorDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string? ProfilePicture { get; set; }
}
