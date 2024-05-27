using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User> Get(Guid id);
    Task<User> Get(string email);
    Task<User> Register(RegisterUserDto registerUserDto);
    Task<bool> Login(LoginUserDto loginUserDto);
    Task<int> Update(Guid id, UpdateUserDto updateUserDto);
}
