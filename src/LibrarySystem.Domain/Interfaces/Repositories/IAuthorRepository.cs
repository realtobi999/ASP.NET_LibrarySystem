using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAll();
    void Create(Author author);
}
