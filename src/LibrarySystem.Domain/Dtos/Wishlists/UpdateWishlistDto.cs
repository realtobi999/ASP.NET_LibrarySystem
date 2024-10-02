using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class UpdateWishlistDto
{
    [Required, MaxLength(55)]
    public string? Name { get; init; }

    [Required]
    public IEnumerable<Guid>? BookIds { get; init; }
}
