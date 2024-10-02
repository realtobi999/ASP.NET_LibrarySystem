using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class EmployeeMapper : IEmployeeMapper
{
    private readonly IHasher _hasher;

    public EmployeeMapper(IHasher hasher)
    {
        _hasher = hasher;
    }

    public Employee CreateFromDto(RegisterEmployeeDto dto)
    {
        return new Employee
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Password = _hasher.Hash(dto.Password ?? throw new NullReferenceException("The password must be set.")),
        };
    }

    public void UpdateFromDto(Employee employee, UpdateEmployeeDto dto)
    {
        employee.Email = dto.Email;
        employee.Name = dto.Name;
    }
}
