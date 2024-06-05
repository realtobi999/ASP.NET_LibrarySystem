using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookRepository
{
    Task<Book?> Get(Guid id);
    void Create(Book book);
}
