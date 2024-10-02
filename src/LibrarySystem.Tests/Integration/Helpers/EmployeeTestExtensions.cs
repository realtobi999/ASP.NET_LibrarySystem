using Bogus;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

public static class EmployeeTestExtensions
{
    private static readonly Faker<Employee> _EmployeeFaker = new Faker<Employee>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password());

    public static Employee WithFakeData(this Employee Employee)
    {
        return _EmployeeFaker.Generate();
    }

    public static RegisterEmployeeDto ToRegisterEmployeeDto(this Employee Employee)
    {
        return new RegisterEmployeeDto
        {
            Id = Employee.Id,
            Name = Employee.Name,
            Email = Employee.Email,
            Password = Employee.Password,
        };
    }
}
