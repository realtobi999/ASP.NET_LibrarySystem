using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class CreateWishlistDto
{
    public Guid? Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required, MaxLength(55)]
    public string? Name { get; set; }

    [Required]
    public IEnumerable<Guid>? BookIds { get; set; }
}
