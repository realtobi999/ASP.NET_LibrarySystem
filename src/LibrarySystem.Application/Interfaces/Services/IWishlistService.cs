using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IWishlistService
{
    Task<Wishlist> Create(CreateWishlistDto createWishlistDto);
}
