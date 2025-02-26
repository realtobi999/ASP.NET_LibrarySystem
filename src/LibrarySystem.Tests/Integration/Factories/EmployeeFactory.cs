using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class EmployeeFactory
{
    private static readonly Faker<Employee> _factory = new Faker<Employee>()
        .RuleFor(e => e.Id, f => f.Random.Guid())
        .RuleFor(e => e.Name, f => f.Internet.UserName())
        .RuleFor(e => e.Email, f => f.Internet.Email())
        .RuleFor(e => e.Password, f => f.Internet.Password())
        .RuleFor(e => e.CreatedAt, _ => DateTimeOffset.UtcNow);

    public static Employee CreateWithFakeData()
    {
        return _factory.Generate();
    }
}
