using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    void CreateUser(User user);
}
