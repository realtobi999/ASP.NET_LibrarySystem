using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

public class StaffRepository : IStaffRepository
{
    private readonly LibrarySystemContext _context;

    public StaffRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Staff staff)
    {
        _context.Staff.Add(staff);
    }

    public Task<Staff?> Get(string email)
    {
        return _context.Staff.FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<IEnumerable<Staff>> GetAll()
    {
        return await _context.Staff.ToListAsync();
    }
}
