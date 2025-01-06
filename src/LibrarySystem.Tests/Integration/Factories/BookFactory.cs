using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class BookFactory
{
    private static readonly Faker<Book> _factory = new Faker<Book>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.ISBN, f => f.Random.Replace("###-#-##-######-#"))
        .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
        .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
        .RuleFor(b => b.PagesCount, f => f.Random.Int(100, 1000))
        .RuleFor(b => b.PublishedDate, f => f.Date.PastOffset())
        .RuleFor(b => b.IsAvailable, true);

    public static Book CreateWithFakeData()
    {
        return _factory.Generate();
    }
}
