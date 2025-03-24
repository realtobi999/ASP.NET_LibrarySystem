using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class WishlistTestExtensions
{
    public static CreateWishlistDto ToCreateWishlistDto(this Wishlist wishlist, IEnumerable<Guid> bookIds)
    {
        return new CreateWishlistDto
        {
            Id = wishlist.Id,
            UserId = wishlist.UserId,
            Name = wishlist.Name,
            BookIds = bookIds
        };
    }
}