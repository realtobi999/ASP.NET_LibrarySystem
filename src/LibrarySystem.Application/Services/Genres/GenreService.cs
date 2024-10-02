using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Genres;

public class GenreService : IGenreService
{
    private readonly IRepositoryManager _repository;

    public GenreService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(Genre genre)
    {
        // create genre entity and save changes
        _repository.Genre.Create(genre);
        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Genre genre)
    {
        // delete genre entity and save changes
        _repository.Genre.Delete(genre);
        await _repository.SaveSafelyAsync();
    }

    public async Task<Genre> GetAsync(Guid id)
    {
        var genre = await _repository.Genre.GetAsync(id) ?? throw new NotFound404Exception(nameof(Genre), id);

        return genre;
    }

    public async Task<IEnumerable<Genre>> IndexAsync()
    {
        var genres = await _repository.Genre.IndexAsync();

        return genres;
    }

    public async Task UpdateAsync(Genre genre)
    {
        // update genre entity and save changes
        _repository.Genre.Update(genre);
        await _repository.SaveSafelyAsync();
    }
}