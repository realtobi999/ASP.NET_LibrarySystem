using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IEmployeeService : IBaseService<Employee>
{
    Task<Employee> GetAsync(string email);
    Task<bool> AuthAsync(LoginEmployeeDto loginEmployeeDto);
}
