using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly LibrarySystemContext _context;

    public WishlistRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Wishlist wishlist)
    {
        _context.Wishlists.Add(wishlist);
    }

    public Task<Wishlist?> Get(Guid id)
    {
        return _context.Wishlists.Include(w => w.WishlistBooks)
                                    .ThenInclude(w => w.Book)
                                 .FirstOrDefaultAsync(w => w.Id == id);
    }
}
