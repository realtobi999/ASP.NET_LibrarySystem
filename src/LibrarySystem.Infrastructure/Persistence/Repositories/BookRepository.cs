using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(LibrarySystemContext context) : base(context)
    {
    }

    public Book? Get(Guid id)
    {
        return _context.Books.FirstOrDefault(b => b.Id == id);
    }

    public async Task<Book?> GetAsync(Guid id)
    {
        return await this.GetAsync(b => b.Id == id);
    }

    public async Task<Book?> GetAsync(string isbn)
    {
        return await this.GetAsync(b => b.ISBN == isbn);
    }

    protected override IQueryable<Book> GetQueryable()
    {
        return base.GetQueryable()
                   .Include(b => b.Authors)
                   .Include(b => b.Genres)
                   .Include(b => b.Borrows)
                   .Include(b => b.BookReviews)
                   .Include(b => b.CoverPictures);
    }
}
