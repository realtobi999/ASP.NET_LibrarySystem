using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly LibrarySystemContext _context;

    public EmployeeRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Employee employee)
    {
        _context.Employee.Add(employee);
    }

    public void Delete(Employee employee)
    {
        _context.Employee.Remove(employee);
    }

    public async Task<Employee?> Get(string email)
    {
        return await _context.Employee.FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<Employee?> Get(Guid id)
    {
        return await _context.Employee.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Employee.ToListAsync();
    }
}
