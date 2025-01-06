using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class UserFactory
{
    private static readonly Faker<User> _factory = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password());

    public static User CreateWithFakeData()
    {
        return _factory.Generate();
    }
}
