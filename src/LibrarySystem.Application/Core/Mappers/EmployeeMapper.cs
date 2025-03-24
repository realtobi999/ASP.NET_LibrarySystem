using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Application.Core.Mappers;

public class EmployeeMapper : IMapper<Employee, RegisterEmployeeDto>
{
    private readonly IHasher _hasher;

    public EmployeeMapper(IHasher hasher)
    {
        _hasher = hasher;
    }

    public Employee Map(RegisterEmployeeDto dto)
    {
        return new Employee
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Password = _hasher.Hash(dto.Password ?? throw new NullReferenceException("The password must be set.")),
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}