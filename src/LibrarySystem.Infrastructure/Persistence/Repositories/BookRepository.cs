using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

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

    public async Task<Book?> Get(Guid id, bool withRelations = true)
    {
        var book = _context.Books.AsQueryable();

        if (withRelations)
        {
            book = book.IncludeBookRelations();
        }

        return await book.Include(b => b.CoverPictures).SingleOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAll(bool withRelations = true)
    {
        var books = _context.Books.AsQueryable();

        if (withRelations)
        {
            books = books.IncludeBookRelations();
        }

        return await books.Include(b => b.CoverPictures).ToListAsync();
    }

    public async Task<Book?> Get(string isbn, bool withRelations = true)
    {
        var book = _context.Books.AsQueryable();

        if (withRelations)
        {
            book = book.IncludeBookRelations();
        }

        return await book.Include(b => b.CoverPictures).SingleOrDefaultAsync(b => b.ISBN == isbn);
    }
}
