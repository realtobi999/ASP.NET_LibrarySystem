using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class WishlistMapper : IMapper<Wishlist, CreateWishlistDto>
{
    public Wishlist Map(CreateWishlistDto dto)
    {
        var wishlist = new Wishlist
        {
            Id = dto.Id ?? Guid.NewGuid(),
            UserId = dto.UserId,
            Name = dto.Name
        };

        // clean previous attached books and assign new
        foreach (var bookId in dto.BookIds)
        {
            wishlist.WishlistBooks.Add(new WishlistBook
            {
                WishlistId = wishlist.Id,
                BookId = bookId,
            });
        }

        return wishlist;
    }
}
