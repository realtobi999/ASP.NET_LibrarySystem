using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;

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

    public async Task<Genre> Get(Guid id)
    {
        var genre = await _repository.Genre.Get(id) ?? throw new GenreNotFoundException(id);

        return genre;
    }
}
