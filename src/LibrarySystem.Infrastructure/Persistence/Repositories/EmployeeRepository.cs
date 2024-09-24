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
        _context.Employees.Add(employee);
    }

    public void Delete(Employee employee)
    {
        _context.Employees.Remove(employee);
    }

    public async Task<Employee?> Get(string email)
    {
        return await _context.Employees.FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<Employee?> Get(Guid id)
    {
        return await _context.Employees.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Employee>> Index()
    {
        return await _context.Employees.ToListAsync();
    }
}
