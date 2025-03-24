using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class EmployeeTestExtensions
{
    public static RegisterEmployeeDto ToRegisterEmployeeDto(this Employee employee)
    {
        return new RegisterEmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email,
            Password = employee.Password
        };
    }
}