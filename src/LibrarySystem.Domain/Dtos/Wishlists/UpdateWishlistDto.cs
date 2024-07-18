using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class UpdateWishlistDto
{
    [Required, MaxLength(55)]
    public string? Name { get; set; }

    public List<Guid>? BookIds { get; set; }
}
