using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    void Create(User user);
    Task<User?> Get(Guid Id);
    Task<User?> Get(string Email);
    Task<IEnumerable<User>> Index();
    void Delete(User user);
}
