using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;

/// <inheritdoc/>
public interface IGenreMapper : IMapper<Genre, CreateGenreDto, UpdateGenreDto>
{

}
