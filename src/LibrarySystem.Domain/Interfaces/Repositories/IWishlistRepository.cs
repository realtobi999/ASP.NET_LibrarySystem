using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IWishlistRepository : IBaseRepository<Wishlist>
{
    Task<Wishlist?> GetAsync(Guid id);
}