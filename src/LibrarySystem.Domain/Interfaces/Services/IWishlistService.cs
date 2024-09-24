using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IWishlistService
{
    Task<Wishlist> Get(Guid id);
    Task<Wishlist> Create(CreateWishlistDto createWishlistDto);
    Task<int> Update(Guid id, UpdateWishlistDto updateWishlistDto);
    Task<int> Update(Wishlist wishlist, UpdateWishlistDto updateWishlistDto);
    Task<int> Delete(Guid id);
    Task<int> Delete(Wishlist wishlist);
}
