using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAll();
    void Create(Employee employee);
    Task<Employee?> Get(string email);
    Task<Employee?> Get(Guid id);
    void Delete(Employee employee);
}
