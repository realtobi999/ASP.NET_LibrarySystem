using LibrarySystem.Domain.Entities.Relationships;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IAssociationsRepository
{
    void CreateBookAuthor(BookAuthor bookAuthor);
    void CreateBookGenre(BookGenre bookGenre);
}
