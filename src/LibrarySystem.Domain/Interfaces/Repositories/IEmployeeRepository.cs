using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAll();
    void Create(Employee Employee);
    Task<Employee?> Get(string email);
}
