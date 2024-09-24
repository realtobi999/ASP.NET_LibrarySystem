using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
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

    public async Task<int> Delete(Guid id)
    {
        var user = await this.Get(id);

        _repository.User.Delete(user);
        return await _repository.SaveAsync();
    }

    public async Task<User> Get(Guid id)
    {
        var user = await _repository.User.Get(id) ?? throw new NotFound404Exception(nameof(User), id);

        return user;
    }

    public async Task<User> Get(string email)
    {
        var user = await _repository.User.Get(email) ?? throw new NotFound404Exception(nameof(User), email);

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var users = await _repository.User.GetAll();

        return users;
    }

    public async Task<bool> Login(LoginUserDto loginUserDto)
    {
        var email = loginUserDto.Email ?? throw new NullReferenceException("The email must be set.");
        var password = loginUserDto.Password ?? throw new NullReferenceException("The password must be set.");

        var user = await this.Get(email);

        return _hasher.Compare(password, user.Password!);
    }

    public async Task<User> Create(RegisterUserDto registerUserDto)
    {
        var user = new User
        {
            Id = registerUserDto.Id ?? Guid.NewGuid(),
            Username = registerUserDto.Username ?? throw new NullReferenceException("The username must be set."),
            Email = registerUserDto.Email ?? throw new NullReferenceException("The email must be set."),
            Password = _hasher.Hash(registerUserDto.Password ?? throw new NullReferenceException("The password must be set."))
        };

        _repository.User.Create(user);
        await _repository.SaveAsync();

        return user;
    }

    public async Task<int> Update(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await this.Get(id);

        var email = updateUserDto.Email;
        var username = updateUserDto.Username;

        user.Email = email;
        user.Username = username;

        return await _repository.SaveAsync();
    }
}
