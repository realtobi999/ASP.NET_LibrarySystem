using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> Index();
    Task<Author?> Get(Guid id);
    void Create(Author author);
    void Delete(Author author);
}
