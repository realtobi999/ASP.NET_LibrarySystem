using System.Net.Http.Headers;
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

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
    }

    public async Task<Book?> Get(Guid id)
    {
        return await GetBooksQuery(true).SingleOrDefaultAsync(b => b.Id == id); 
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
        return await GetBooksQuery(true).ToListAsync(); 
    }

    public async Task<Book?> Get(string isbn)
    {
        return await GetBooksQuery(true).SingleOrDefaultAsync(b => b.ISBN == isbn);
    }

    private IQueryable<Book> GetBooksQuery(bool includeRelations = true)
    {
        var query = _context.Books.AsQueryable();

        if (includeRelations)
        {
            query = query.Include(b => b.BookAuthors)
                             .ThenInclude(ba => ba.Author)
                         .Include(b => b.BookGenres)
                            .ThenInclude(bg => bg.Genre)
                         .Include(b => b.BookReviews);
        }

        return query;
    }
}
