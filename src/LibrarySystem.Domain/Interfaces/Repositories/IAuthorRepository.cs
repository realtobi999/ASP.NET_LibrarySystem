using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IAuthorRepository
{
    void Create(Author author);
}
