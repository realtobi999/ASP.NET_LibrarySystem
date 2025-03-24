using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookRepository : IBaseRepository<Book>
{
    Book? Get(Guid id);
    Task<Book?> GetAsync(Guid id);
    Task<Book?> GetAsync(string isbn);
}