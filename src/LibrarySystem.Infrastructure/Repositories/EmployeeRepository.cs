using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

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
