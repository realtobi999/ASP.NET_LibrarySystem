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

    public IEmployeeRepository Employee => _factory.CreateEmployeeRepository();

    public IAuthorRepository Author => _factory.CreateAuthorRepository();

    public IGenreRepository Genre => _factory.CreateGenreRepository();

    public IAssociationsRepository Associations => _factory.CreateAssociationsRepository();

    public IBookRepository Book => _factory.CreateBookRepository();

    public IBorrowRepository Borrow => _factory.CreateBorrowRepository(); 

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}
