using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class UpdateWishlistDto
{
    [Required, MaxLength(55)]
    public required string Name { get; init; }

    [Required]
    public required IEnumerable<Guid> BookIds { get; init; } = [];
}
