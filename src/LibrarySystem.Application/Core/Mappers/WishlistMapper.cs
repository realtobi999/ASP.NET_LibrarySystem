using LibrarySystem.Application.Services.Wishlists;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class WishlistMapper : IWishlistMapper
{
    private readonly IWishlistAssociations _associations;

    public WishlistMapper(IWishlistAssociations associations)
    {
        _associations = associations;
    }

    public Wishlist CreateFromDto(CreateWishlistDto dto)
    {
        var wishlist = new Wishlist
        {
            Id = dto.Id ?? Guid.NewGuid(),
            UserId = dto.UserId,
            Name = dto.Name
        };

        // null check and assign books
        if (dto.BookIds is not null)
        {
            _associations.AssignBooks(dto.BookIds, wishlist);
        }

        return wishlist;
    }

    public void UpdateFromDto(Wishlist wishlist, UpdateWishlistDto dto)
    {
        wishlist.Name = dto.Name;

        // check if the update request book property is null, if not clean the previous and assign the new
        if (dto.BookIds is not null)
        {
            _associations.CleanBooks(wishlist);

            _associations.AssignBooks(dto.BookIds, wishlist);
        }
    }
}
