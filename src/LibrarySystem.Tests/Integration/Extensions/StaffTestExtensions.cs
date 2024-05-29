using Bogus;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests;

public static class StaffTestExtensions
{
    private static readonly Faker<Staff> _staffFaker = new Faker<Staff>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password());

    public static Staff WithFakeData(this Staff staff)
    {
        return _staffFaker.Generate();
    }

    public static RegisterStaffDto ToRegisterStaffDto(this Staff staff)
    {
        return new RegisterStaffDto
        {
            Id = staff.Id,
            Name = staff.Name,
            Email = staff.Email,
            Password = staff.Password,
        };
    }
}
