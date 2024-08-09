using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly IPasswordHasher _hasher;

    public EmployeeService(IRepositoryManager repository, IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public async Task<Employee> Get(string email)
    {
        var employee = await _repository.Employee.Get(email) ?? throw new NotFound404Exception(nameof(Employee), email);

        return employee;
    }

    public async Task<bool> Login(LoginEmployeeDto loginEmployeeDto)
    {
        var email = loginEmployeeDto.Email ?? throw new NullReferenceException("The email must be set.");
        var password = loginEmployeeDto.Password ?? throw new NullReferenceException("The password must be set."); 

        var employee = await this.Get(email);

        return _hasher.Compare(password, employee.Password!);
    }

    public async Task<Employee> Create(RegisterEmployeeDto registerEmployeeDto)
    {
        var employee = new Employee
        {
            Id = registerEmployeeDto.Id ?? Guid.NewGuid(),
            Name = registerEmployeeDto.Name,
            Email = registerEmployeeDto.Email,
            Password = _hasher.Hash(registerEmployeeDto.Password ?? throw new NullReferenceException("The password must be set.")),
        };

        _repository.Employee.Create(employee);
        await _repository.SaveAsync();

        return employee;
    }

    public Task<IEnumerable<Employee>> GetAll()
    {
        var employees = _repository.Employee.GetAll();

        return employees;
    }

    public async Task<Employee> Get(Guid id)
    {
        var employee = await _repository.Employee.Get(id) ?? throw new NotFound404Exception(nameof(Employee), id);

        return employee;
    }

    public async Task<int> Update(Guid id, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await this.Get(id);

        var name = updateEmployeeDto.Name;
        var email = updateEmployeeDto.Email;

        employee.Email = email;
        employee.Name = name;

        return await _repository.SaveAsync();
    }

    public async Task<int> Delete(Guid id)
    {
        var employee = await this.Get(id);

        _repository.Employee.Delete(employee);
        return await _repository.SaveAsync();
    }
}
