using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAll(bool relations = true);
    Task<Book?> Get(Guid id, bool relations = true);
    Task<Book?> Get(string isbn, bool relations = true);
    void Create(Book book);
    void Delete(Book book);
}
