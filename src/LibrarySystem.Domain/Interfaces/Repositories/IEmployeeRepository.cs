using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAll();
    void Create(Employee employee);
    Task<Employee?> Get(string email);
    Task<Employee?> Get(Guid id);
}
