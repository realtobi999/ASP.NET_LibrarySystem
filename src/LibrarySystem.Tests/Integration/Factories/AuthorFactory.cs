using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class AuthorFactory
{
    private static readonly Faker<Author> _factory = new Faker<Author>()
        .RuleFor(a => a.Id, f => f.Random.Guid())
        .RuleFor(a => a.Name, f => f.Name.FullName())
        .RuleFor(a => a.Description, f => f.Lorem.Paragraph())
        .RuleFor(a => a.Birthday, f => f.Date.Past(50, DateTime.Now.AddYears(-20))); // Assuming authors are at least 20 years old

    public static Author CreateWithFakeData()
    {
        return _factory.Generate();
    }
}
