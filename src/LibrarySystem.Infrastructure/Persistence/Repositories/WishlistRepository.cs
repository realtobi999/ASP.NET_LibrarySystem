using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
{
    public WishlistRepository(LibrarySystemContext context) : base(context)
    {
    }

    public async Task<Wishlist?> GetAsync(Guid id)
    {
        return await this.GetAsync(w => w.Id == id);
    }

    protected override IQueryable<Wishlist> GetQueryable()
    {
        return base.GetQueryable()
                   .Include(w => w.WishlistBooks)
                       .ThenInclude(wb => wb.Book)
                       .ThenInclude(b => b!.BookAuthors)
                           .ThenInclude(ba => ba.Author)
                   .Include(w => w.WishlistBooks)
                       .ThenInclude(wb => wb.Book)
                       .ThenInclude(b => b!.BookGenres)
                           .ThenInclude(bg => bg.Genre)
                   .Include(w => w.WishlistBooks)
                       .ThenInclude(wb => wb.Book)
                       .ThenInclude(b => b!.BookReviews)
                   .Include(w => w.WishlistBooks)
                       .ThenInclude(wb => wb.Book)
                       .ThenInclude(b => b!.CoverPictures);
    }

}
