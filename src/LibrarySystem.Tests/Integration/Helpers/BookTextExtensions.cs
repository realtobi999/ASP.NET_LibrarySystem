using System.Net.Http.Json;
using Bogus;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

public static class BookTestExtensions
{
    private static readonly Faker<Book> _BookFaker = new Faker<Book>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.ISBN, f => f.Random.Replace("###-#-##-######-#"))
        .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
        .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
        .RuleFor(b => b.PagesCount, f => f.Random.Int(100, 1000))
        .RuleFor(b => b.PublishedDate, f => f.Date.PastOffset())
        .RuleFor(b => b.IsAvailable, true);

    public static Book WithFakeData(this Book book)
    {
        return _BookFaker.Generate();
    }

    public static async Task<CreateBookDto> ToCreateBookDtoWithGenresAndAuthorsAsync(this Book book, HttpClient client)
    {
        var authors = new List<Author>() { new Author().WithFakeData(), new Author().WithFakeData() };
        var genres = new List<Genre>() { new Genre().WithFakeData(), new Genre().WithFakeData() };

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