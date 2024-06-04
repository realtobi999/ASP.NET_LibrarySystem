using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

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
}
