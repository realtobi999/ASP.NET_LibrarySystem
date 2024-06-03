using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAll();
    Task<Genre?> Get(Guid id);
    void Create(Genre genre);
    void Delete(Genre genre);
}
