using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IPasswordHasher _hasher;

    public UserService(IRepositoryManager repository, IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _repository.User.GetUserByEmail(email) ?? throw new NotFoundException($"The user with email {email} doesnt exist.");

        return user;
    }

    public async Task<bool> LoginUser(LoginUserDto loginUserDto)
    {
        var email = loginUserDto.Email ?? throw new ArgumentNullException("The email must be set.");
        var password = loginUserDto.Password ?? throw new ArgumentNullException("The password must be set."); 

        var user = await _repository.User.GetUserByEmail(email) ?? throw new NotFoundException($"The user with email {email} doesnt exist.");

        return _hasher.Compare(password, user.Password!);
    }

    public async Task<User> RegisterUser(RegisterUserDto registerUserDto)
    {
        var user = new User
        {
            Id = registerUserDto.Id ?? Guid.NewGuid(),
            Username = registerUserDto.Username,
            Email = registerUserDto.Email,
            Password = _hasher.Hash(registerUserDto.Password!)
        };

        _repository.User.CreateUser(user);
        await _repository.SaveAsync();

        return user;
    }
}
