using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> Index(bool withRelations = true);
    Task<Book?> Get(Guid id, bool withRelations = true);
    Task<Book?> Get(string isbn, bool withRelations = true);
    void Create(Book book);
    void Delete(Book book);
}
