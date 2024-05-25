using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly LibrarySystemContext _context;

    public UserRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
    }
}
