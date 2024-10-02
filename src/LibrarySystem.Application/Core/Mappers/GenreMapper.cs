using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class GenreMapper : IGenreMapper
{
    public Genre CreateFromDto(CreateGenreDto dto)
    {
        return new Genre
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Name = dto.Name,
        };
    }

    public void UpdateFromDto(Genre genre, UpdateGenreDto dto)
    {
        genre.Name = dto.Name;
    }
}
