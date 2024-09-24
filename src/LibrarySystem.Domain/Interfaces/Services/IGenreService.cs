using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IGenreService
{
    Task<IEnumerable<Genre>> Index();
    Task<Genre> Get(Guid id);
    Task<Genre> Create(CreateGenreDto createGenreDto);
    Task<int> Update(Guid id, UpdateGenreDto updateGenreDto);
    Task<int> Delete(Guid id);
}
