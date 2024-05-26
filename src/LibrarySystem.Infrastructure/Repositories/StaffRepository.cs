using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Infrastructure;

public class StaffRepository : IStaffRepository
{
    private readonly LibrarySystemContext _context;

    public StaffRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void CreateStaff(Staff staff)
    {
        _context.Staff.Add(staff);
    }
}
