using Bogus;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

public static class GenreTestExtensions
{
    private static readonly Faker<Genre> _genreFaker = new Faker<Genre>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => f.Internet.UserName());

    public static Genre WithFakeData(this Genre genre)
    {
        return _genreFaker.Generate();
    }

    public static CreateGenreDto ToCreateGenreDto(this Genre genre)
    {
        return new CreateGenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
        };
    }
}
