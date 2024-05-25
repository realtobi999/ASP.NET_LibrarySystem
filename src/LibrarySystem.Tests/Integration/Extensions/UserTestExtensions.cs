using Bogus;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Extensions;

public static class UserTestExtensions
{
    private static readonly Faker<User> _userFaker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password());

    public static User WithFakeData(this User user)
    {
        return _userFaker.Generate();
    }

    public static RegisterUserDto ToRegisterUserDto(this User user)
    {
        return new RegisterUserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password
        };
    }
}
