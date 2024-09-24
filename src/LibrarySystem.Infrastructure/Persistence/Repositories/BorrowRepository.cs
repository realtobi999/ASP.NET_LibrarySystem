using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly LibrarySystemContext _context;

    public BorrowRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(Borrow borrow)
    {
        _context.Borrows.Add(borrow);
    }

    public async Task<Borrow?> Get(Guid id)
    {
        return await _context.Borrows.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Borrow?> Get(Guid bookId, Guid userId)
    {
        return await _context.Borrows.FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId);
    }

    public async Task<IEnumerable<Borrow>> Index()
    {
        return await _context.Borrows.OrderBy(b => b.BorrowDate).ToListAsync();
    }
}
