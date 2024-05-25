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

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
    }

    public async Task<User?> GetUser(Guid Id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<User?> GetUserByEmail(string Email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }
}
