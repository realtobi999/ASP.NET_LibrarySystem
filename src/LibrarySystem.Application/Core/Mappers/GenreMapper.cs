using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Application.Core.Mappers;

public class GenreMapper : IMapper<Genre, CreateGenreDto>
{
    public Genre Map(CreateGenreDto dto)
    {
        return new Genre
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Name = dto.Name,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}