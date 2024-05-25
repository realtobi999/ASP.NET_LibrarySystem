using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IRepositoryFactory _factory;
    private readonly LibrarySystemContext _context;

    public RepositoryManager(IRepositoryFactory factory, LibrarySystemContext context)
    {
        _factory = factory;
        _context = context;
    }

    public IUserRepository User => _factory.CreateUserRepository();

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}
