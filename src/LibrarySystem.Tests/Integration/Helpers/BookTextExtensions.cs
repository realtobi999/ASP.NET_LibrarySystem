using System.Net.Http.Json;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Factories;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class BookTestExtensions
{
    public static async Task<CreateBookDto> ToCreateBookDtoWithGenresAndAuthorsAsync(this Book book, HttpClient client)
    {
        var authors = new List<Author>() { AuthorFactory.CreateWithFakeData(), AuthorFactory.CreateWithFakeData() };
        var genres = new List<Genre>() { GenreFactory.CreateWithFakeData(), GenreFactory.CreateWithFakeData() };

        foreach (var author in authors)
        {
            var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
            create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        foreach (var genre in genres)
        {
            var create = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
            create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        return book.ToCreateBookDto(authors.Select(a => a.Id), genres.Select(g => g.Id));
    }

    public static CreateBookDto ToCreateBookDto(this Book book, IEnumerable<Guid> authorIds, IEnumerable<Guid> genreIds)
    {
        return new CreateBookDto
        {
            Id = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Description = book.Description,
            PagesCount = book.PagesCount,
            PublishedDate = book.PublishedDate,
            Available = book.IsAvailable,
            AuthorIds = authorIds,
            GenreIds = genreIds,
        };
    }
}