using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<User>> Index();
    Task<User> Get(Guid id);
    Task<User> Get(string email);
    Task<User> Create(RegisterUserDto registerUserDto);
    Task<bool> Login(LoginUserDto loginUserDto);
    Task<int> Update(Guid id, UpdateUserDto updateUserDto);
    Task<int> Delete(Guid id);
}
