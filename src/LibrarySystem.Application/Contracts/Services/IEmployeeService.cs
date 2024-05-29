using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAll();
    Task<Employee> Create(RegisterEmployeeDto registerEmployeeDto);
    Task<bool> Login(LoginEmployeeDto loginEmployeeDto);
    Task<Employee> Get(string email);
    Task<Employee> Get(Guid id);
}
