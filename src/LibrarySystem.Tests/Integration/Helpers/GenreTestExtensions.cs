using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class GenreTestExtensions
{
    public static CreateGenreDto ToCreateGenreDto(this Genre genre)
    {
        return new CreateGenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
        };
    }
}
