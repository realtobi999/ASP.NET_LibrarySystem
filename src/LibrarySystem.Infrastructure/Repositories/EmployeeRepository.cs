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

    public void Create(Employee Employee)
    {
        _context.Employee.Add(Employee);
    }

    public Task<Employee?> Get(string email)
    {
        return _context.Employee.FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Employee.ToListAsync();
    }
}
