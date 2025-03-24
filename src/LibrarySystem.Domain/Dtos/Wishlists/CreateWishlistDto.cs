using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record CreateWishlistDto
{
    public Guid? Id { get; init; }

    [Required]
    public required Guid UserId { get; init; }

    [Required, MaxLength(55)]
    public required string Name { get; init; }

    [Required]
    public required IEnumerable<Guid> BookIds { get; init; } = [];
}