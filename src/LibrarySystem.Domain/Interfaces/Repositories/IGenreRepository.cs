using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IGenreRepository
{
    void Create(Genre genre);
}
