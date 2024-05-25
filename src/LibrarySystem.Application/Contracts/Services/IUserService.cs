using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IUserService
{
    Task<User> GetUserByEmail(string email);
    Task<User> RegisterUser(RegisterUserDto registerUserDto);
    Task<bool> LoginUser(LoginUserDto loginUserDto);
}
