using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee> Get(Guid id);
    Task<Employee> Get(string email);
    Task<Employee> Create(RegisterEmployeeDto registerEmployeeDto);
    Task<bool> Login(LoginEmployeeDto loginEmployeeDto);
    Task<int> Update(Guid id, UpdateEmployeeDto updateEmployeeDto);
    Task<int> Delete(Guid id);
}
