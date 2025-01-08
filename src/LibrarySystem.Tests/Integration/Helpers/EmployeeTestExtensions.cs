using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class EmployeeTestExtensions
{
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
