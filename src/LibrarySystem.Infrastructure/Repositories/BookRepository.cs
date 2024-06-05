using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibrarySystemContext _context;

    public BookRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Book book)
    {
        _context.Books.Add(book);
    }

    public async Task<Book?> Get(Guid id)
    {
        return await _context.Books
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .SingleOrDefaultAsync(h => h.Id == id);
    }
}
