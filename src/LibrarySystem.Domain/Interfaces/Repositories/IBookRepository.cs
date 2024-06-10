using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAll();
    Task<Book?> Get(Guid id);
    Task<Book?> Get(string isbn);
    void Create(Book book);
    void Delete(Book book);
}
