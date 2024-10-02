using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;


/// <inheritdoc/>
public interface IEmployeeMapper : IMapper<Employee, RegisterEmployeeDto, UpdateEmployeeDto>
{

}
