using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LibrarySystemContext context) : base(context)
    {
    }

    public async Task<User?> GetAsync(Guid id)
    {
        return await this.GetAsync(u => u.Id == id);
    }

    public async Task<User?> GetAsync(string email)
    {
        return await this.GetAsync(u => u.Email == email);
    }
}
