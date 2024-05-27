using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUser(Guid id);
    Task<User> GetUserByEmail(string email);
    Task<User> RegisterUser(RegisterUserDto registerUserDto);
    Task<bool> LoginUser(LoginUserDto loginUserDto);
}
