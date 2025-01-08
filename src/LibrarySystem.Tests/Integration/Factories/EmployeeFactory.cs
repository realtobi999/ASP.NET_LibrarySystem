using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class EmployeeFactory
{
    private static readonly Faker<Employee> _factory = new Faker<Employee>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password());

    public static Employee CreateWithFakeData()
    {
        return _factory.Generate();
    }
}
