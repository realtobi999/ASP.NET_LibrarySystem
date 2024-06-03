using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Contracts.Services;

public interface IGenreService
{
    Task<IEnumerable<Genre>> GetAll();
    Task<Genre> Get(Guid id);
    Task<Genre> Create(CreateGenreDto createGenreDto);
    Task<int> Update(Guid id, UpdateGenreDto updateGenreDto);
    Task<int> Delete(Guid id);
}
