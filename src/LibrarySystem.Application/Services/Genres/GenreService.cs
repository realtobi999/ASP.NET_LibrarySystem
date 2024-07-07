using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.NotFound;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Services.Genres;

public class GenreService : IGenreService
{
    private readonly IRepositoryManager _repository;

    public GenreService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Genre> Create(CreateGenreDto createGenreDto)
    {
        var genre = new Genre
        {
            Id = createGenreDto.Id ?? Guid.NewGuid(),
            Name = createGenreDto.Name ?? throw new ArgumentNullException("The name must be set.")
        };

        _repository.Genre.Create(genre);
        await _repository.SaveAsync();

        return genre;
    }

    public async Task<int> Delete(Guid id)
    {
        var genre = await _repository.Genre.Get(id) ?? throw new GenreNotFoundException(id);

        _repository.Genre.Delete(genre);
        return await _repository.SaveAsync();
    }

    public async Task<Genre> Get(Guid id)
    {
        var genre = await _repository.Genre.Get(id) ?? throw new GenreNotFoundException(id);

        return genre;
    }

    public async Task<IEnumerable<Genre>> GetAll()
    {
       var genres = await _repository.Genre.GetAll();

       return genres; 
    }

    public async Task<int> Update(Guid id, UpdateGenreDto updateGenreDto)
    {
        var genre = await _repository.Genre.Get(id) ?? throw new GenreNotFoundException(id);

        var name = updateGenreDto.Name;

        if (!name.IsNullOrEmpty())
        {
            genre.Name = name;
        }

        return await _repository.SaveAsync();
    }
}