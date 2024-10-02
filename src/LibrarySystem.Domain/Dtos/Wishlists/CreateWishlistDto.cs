using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class CreateWishlistDto
{
    public Guid? Id { get; init; }

    [Required]
    public Guid UserId { get; init; }

    [Required, MaxLength(55)]
    public string? Name { get; init; }

    [Required]
    public IEnumerable<Guid> BookIds { get; init; } = [];
}
