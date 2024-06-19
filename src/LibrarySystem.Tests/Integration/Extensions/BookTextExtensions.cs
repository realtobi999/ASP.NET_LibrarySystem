using Bogus;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Extensions;

public static class BookTestExtensions
{
    private static readonly Faker<Book> _BookFaker = new Faker<Book>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.ISBN, f => f.Random.Replace("###-#-##-######-#"))
        .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
        .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
        .RuleFor(b => b.PagesCount, f => f.Random.Int(100, 1000))
        .RuleFor(b => b.PublishedDate, f => f.Date.PastOffset())
        .RuleFor(b => b.IsAvailable , true)
        .RuleFor(b => b.CoverPicture, f => f.Internet.Url());

    public static Book WithFakeData(this Book book)
    {
        return _BookFaker.Generate();
    }

    public static CreateBookDto ToCreateBookDto(this Book book)
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
            CoverPicture = book.CoverPicture,
        };
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
            CoverPicture = book.CoverPicture,
            AuthorIds = authorIds,
            GenreIds = genreIds,
        };
    }
}