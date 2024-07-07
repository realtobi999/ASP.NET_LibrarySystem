using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IWishlistService
{
    Task<Wishlist> Get(Guid id);
    Task<Wishlist> Create(CreateWishlistDto createWishlistDto);
    Task<int> Update(Guid id, UpdateWishlistDto updateWishlistDto);
    Task<int> Update(Wishlist wishlist, UpdateWishlistDto updateWishlistDto);
}
