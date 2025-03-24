using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<Employee?> GetAsync(string email);
    Task<Employee?> GetAsync(Guid id);
}