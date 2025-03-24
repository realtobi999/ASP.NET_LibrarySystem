using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal static class BookFactory
{
    private static readonly Faker<Book> Factory = new Faker<Book>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.Isbn, f => f.Random.Replace("###-#-##-######-#"))
        .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
        .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
        .RuleFor(b => b.PagesCount, f => f.Random.Int(100, 1000))
        .RuleFor(b => b.PublishedDate, f => f.Date.PastOffset())
        .RuleFor(b => b.Popularity, f => f.Random.Double(0, 100))
        .RuleFor(b => b.IsAvailable, true)
        .RuleFor(b => b.CreatedAt, _ => DateTimeOffset.UtcNow);

    public static Book CreateWithFakeData()
    {
        return Factory.Generate();
    }
}