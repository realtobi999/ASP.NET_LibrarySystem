using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class AssociationsRepository : IAssociationsRepository
{
    private readonly LibrarySystemContext _context;

    public AssociationsRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void CreateBookAuthor(BookAuthor bookAuthor)
    {
        _context.BookAuthors.Add(bookAuthor);
    }

    public void CreateBookGenre(BookGenre bookGenre)
    {
        _context.BookGenres.Add(bookGenre);
    }

    public void CreateWishlistBook(WishlistBook wishlistBook)
    {
        _context.WishlistBooks.Add(wishlistBook);
    }

    public void RemoveBookAuthor(BookAuthor bookAuthor)
    {
        _context.BookAuthors.Remove(bookAuthor);
    }

    public void RemoveBookGenre(BookGenre bookGenre)
    {
        _context.BookGenres.Remove(bookGenre);
    }

    public void RemoveWishlistBook(WishlistBook wishlistBook)
    {
        _context.WishlistBooks.Remove(wishlistBook);
    }
}
