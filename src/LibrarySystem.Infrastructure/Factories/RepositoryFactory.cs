using LibrarySystem.Domain;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly LibrarySystemContext _context;

    public RepositoryFactory(LibrarySystemContext context)
    {
        _context = context;
    }

    public IAuthorRepository CreateAuthorRepository()
    {
        return new AuthorRepository(_context);
    }

    public IEmployeeRepository CreateEmployeeRepository()
    {
        return new EmployeeRepository(_context);
    }

    public IUserRepository CreateUserRepository()
    {
        return new UserRepository(_context);
    }
}
