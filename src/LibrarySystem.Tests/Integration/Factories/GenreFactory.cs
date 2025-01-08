using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class GenreFactory
{
    private static readonly Faker<Genre> _factory = new Faker<Genre>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => f.Internet.UserName());

    public static Genre CreateWithFakeData()
    {
        return _factory.Generate();
    }
}
