using LibrarySystem.Domain.Entities.Relationships;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IAssociationsRepository
{
    void CreateBookAuthor(BookAuthor bookAuthor);
    void CreateBookGenre(BookGenre bookGenre);
    void RemoveBookAuthor(BookAuthor bookAuthor);
    void RemoveBookGenre(BookGenre bookGenre);
    void CreateWishlistBook(WishlistBook wishlistBook); 
    void RemoveWishlistBook(WishlistBook wishlistBook);
}
