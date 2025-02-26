using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class GenreFactory
{
    private static readonly Faker<Genre> _factory = new Faker<Genre>()
        .RuleFor(g => g.Id, f => f.Random.Guid())
        .RuleFor(g => g.Name, f => f.Lorem.Word())
        .RuleFor(g => g.CreatedAt, _ => DateTimeOffset.UtcNow);

    public static Genre CreateWithFakeData()
    {
        return _factory.Generate();
    }
}
