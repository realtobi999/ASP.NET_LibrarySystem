﻿using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
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
        var employee = await _repository.Employee.Get(email) ?? throw new NotFoundException($"The Employee with email {email} doesnt exist.");

        return employee;
    }

    public async Task<bool> Login(LoginEmployeeDto loginEmployeeDto)
    {
        var email = loginEmployeeDto.Email ?? throw new ArgumentNullException("The email must be set.");
        var password = loginEmployeeDto.Password ?? throw new ArgumentNullException("The password must be set."); 

        var employee = await _repository.Employee.Get(email) ?? throw new NotFoundException($"The Employee with email {email} doesnt exist.");

        return _hasher.Compare(password, employee.Password!);
    }

    public async Task<Employee> Create(RegisterEmployeeDto registerEmployeeDto)
    {
        var employee = new Employee
        {
            Id = registerEmployeeDto.Id ?? Guid.NewGuid(),
            Name = registerEmployeeDto.Name ?? throw new ArgumentNullException("The name must be set."),
            Email = registerEmployeeDto.Email ?? throw new ArgumentNullException("The email must be set."),
            Password = _hasher.Hash(registerEmployeeDto.Password ?? throw new ArgumentNullException("The password must be set.")),
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
        var employee = await _repository.Employee.Get(id) ?? throw new EmployeeNotFoundException(id);

        return employee;
    }
}
