using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IGenreRepository
{
    Task<Genre?> Get(Guid id);
    void Create(Genre genre);
}
