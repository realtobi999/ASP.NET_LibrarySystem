using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class BaseRepository : IBaseRepository
{
    public LibrarySystemContext _context;

    public BaseRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}
