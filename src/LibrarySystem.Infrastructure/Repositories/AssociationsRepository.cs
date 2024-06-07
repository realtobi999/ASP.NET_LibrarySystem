using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Repositories;

public class AssociationsRepository : IAssociationsRepository
{
    private readonly LibrarySystemContext _context;

    public AssociationsRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void CreateBookAuthor(BookAuthor bookAuthor)
    {
        _context.BookAuthor.Add(bookAuthor);
    }

    public void CreateBookGenre(BookGenre bookGenre)
    {
        _context.BookGenre.Add(bookGenre);
    }

    public void RemoveBookAuthor(BookAuthor bookAuthor)
    {
        _context.BookAuthor.Remove(bookAuthor);
    }

    public void RemoveBookGenre(BookGenre bookGenre)
    {
        _context.BookGenre.Remove(bookGenre);
    }
}
