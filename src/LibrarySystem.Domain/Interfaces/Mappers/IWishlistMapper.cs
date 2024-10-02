using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;

/// <inheritdoc/>
public interface IWishlistMapper : IMapper<Wishlist, CreateWishlistDto, UpdateWishlistDto>
{

}
