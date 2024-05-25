using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
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

    public async Task<User> RegisterUser(RegisterUserDto registerUserDto)
    {
        var user = new User(){
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
