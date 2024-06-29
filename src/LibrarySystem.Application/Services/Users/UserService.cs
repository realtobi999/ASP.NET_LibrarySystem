using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Utilities;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<int> Delete(Guid id)
    {
        var user = await _repository.User.Get(id) ?? throw new UserNotFoundException(id);

        _repository.User.Delete(user);
        return await _repository.SaveAsync();
    }

    public async Task<User> Get(Guid id)
    {
        var user = await _repository.User.Get(id) ?? throw new UserNotFoundException(id);

        return user;
    }

    public async Task<User> Get(string email)
    {
        var user = await _repository.User.Get(email) ?? throw new NotFoundException($"The user with email {email} doesnt exist.");

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var users = await _repository.User.GetAll();

        return users;
    }

    public async Task<bool> Login(LoginUserDto loginUserDto)
    {
        var email = loginUserDto.Email ?? throw new ArgumentNullException("The email must be set.");
        var password = loginUserDto.Password ?? throw new ArgumentNullException("The password must be set."); 

        var user = await _repository.User.Get(email) ?? throw new NotFoundException($"The user with email {email} doesnt exist.");

        return _hasher.Compare(password, user.Password!);
    }

    public async Task<User> Create(RegisterUserDto registerUserDto)
    {
        var user = new User
        {
            Id = registerUserDto.Id ?? Guid.NewGuid(),
            Username = registerUserDto.Username ?? throw new ArgumentNullException("The username must be set."),
            Email = registerUserDto.Email ?? throw new ArgumentNullException("The email must be set."),
            Password = _hasher.Hash(registerUserDto.Password ?? throw new ArgumentNullException("The password must be set."))
        };

        _repository.User.Create(user);
        await _repository.SaveAsync();

        return user;
    }

    public async Task<int> Update(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _repository.User.Get(id) ?? throw new UserNotFoundException(id);

        var email = updateUserDto.Email;
        var username = updateUserDto.Username;

        if (!email.IsNullOrEmpty())
        {
            user.Email = email;
        }
        if (!username.IsNullOrEmpty())
        {
            user.Username = username;
        }

        return await _repository.SaveAsync();
    }
}
