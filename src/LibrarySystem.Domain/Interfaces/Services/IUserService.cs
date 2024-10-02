using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IUserService : IBaseService<User>
{
    Task<User> GetAsync(string email);
    Task<bool> AuthAsync(LoginUserDto loginUserDto);
}
