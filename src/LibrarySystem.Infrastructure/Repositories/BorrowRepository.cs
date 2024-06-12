using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly LibrarySystemContext _context;

    public BorrowRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Borrow borrow)
    {
        _context.Borrow.Add(borrow);
    }

    public async Task<IEnumerable<Borrow>> GetAll()
    {
        return await _context.Borrow.ToListAsync();
    }
}
