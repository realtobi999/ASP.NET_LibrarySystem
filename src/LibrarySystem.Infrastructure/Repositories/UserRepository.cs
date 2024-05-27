using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly LibrarySystemContext _context;

    public UserRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
    }

    public async Task<User?> Get(Guid Id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<User?> Get(string Email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }
}
