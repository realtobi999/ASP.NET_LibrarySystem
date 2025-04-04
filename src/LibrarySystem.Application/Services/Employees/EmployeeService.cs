﻿using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly IHasher _hasher;

    public EmployeeService(IRepositoryManager repository, IHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public async Task<Employee> GetAsync(string email)
    {
        var employee = await _repository.Employee.GetAsync(email) ?? throw new NotFound404Exception(nameof(Employee), email);

        return employee;
    }

    public async Task CreateAsync(Employee employee)
    {
        // create employee and save changes
        _repository.Employee.Create(employee);
        await _repository.SaveSafelyAsync();
    }

    public Task<IEnumerable<Employee>> IndexAsync()
    {
        var employees = _repository.Employee.IndexAsync();

        return employees;
    }

    public async Task<Employee> GetAsync(Guid id)
    {
        var employee = await _repository.Employee.GetAsync(id) ?? throw new NotFound404Exception(nameof(Employee), id);

        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        // update employee and save changes
        _repository.Employee.Update(employee);
        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        // delete employee and save changes
        _repository.Employee.Delete(employee);
        await _repository.SaveSafelyAsync();
    }

    public async Task<bool> AuthAsync(LoginEmployeeDto loginEmployeeDto)
    {
        var email = loginEmployeeDto.Email ?? throw new NullReferenceException("The email must be set.");
        var password = loginEmployeeDto.Password ?? throw new NullReferenceException("The password must be set.");

        var employee = await GetAsync(email);

        if (employee.IsLocked())
        {
            throw new BadRequest400Exception("User is locked. Try again later.");
        }
        else
        {
            employee.Unlock();
        }

        var authenticated = _hasher.Compare(password, employee.Password);

        if (!authenticated)
        {
            employee.LoginAttempts++;

            // lock employee if failed attempts exceed the limit
            if (employee.LoginAttempts % User.AttemptsBeforeLock == 0)
            {
                employee.Lock();
            }
        }
        else
        {
            employee.LoginAttempts = 0; // reset on successful login
        }

        await this.UpdateAsync(employee);

        return authenticated;
    }
}