using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal static class GenreFactory
{
    private static readonly Faker<Genre> Factory = new Faker<Genre>()
        .RuleFor(g => g.Id, f => f.Random.Guid())
        .RuleFor(g => g.Name, f => f.Lorem.Word())
        .RuleFor(g => g.CreatedAt, _ => DateTimeOffset.UtcNow);

    public static Genre CreateWithFakeData()
    {
        return Factory.Generate();
    }
}