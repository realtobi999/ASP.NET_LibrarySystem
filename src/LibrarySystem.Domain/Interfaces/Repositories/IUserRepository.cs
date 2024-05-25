using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    void CreateUser(User user);
    Task<User?> GetUser(Guid Id);
    Task<User?> GetUserByEmail(string Email);
}
