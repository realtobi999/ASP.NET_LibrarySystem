using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> Index();
    Task<Genre?> Get(Guid id);
    void Create(Genre genre);
    void Delete(Genre genre);
}
