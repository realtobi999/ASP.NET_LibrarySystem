using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IWishlistRepository
{
    Task<Wishlist?> Get(Guid id);
    void Create(Wishlist wishlist);
}
