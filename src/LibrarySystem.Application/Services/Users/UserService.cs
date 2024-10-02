using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IHasher _hasher;

    public UserService(IRepositoryManager repository, IHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public async Task DeleteAsync(User user)
    {
        // delete user and save changes
        _repository.User.Delete(user);
        await _repository.SaveSafelyAsync();
    }

    public async Task<User> GetAsync(Guid id)
    {
        var user = await _repository.User.GetAsync(id) ?? throw new NotFound404Exception(nameof(User), id);

        return user;
    }

    public async Task<User> GetAsync(string email)
    {
        var user = await _repository.User.GetAsync(email) ?? throw new NotFound404Exception(nameof(User), email);

        return user;
    }

    public async Task<IEnumerable<User>> IndexAsync()
    {
        var users = await _repository.User.IndexAsync();

        return users;
    }

    public async Task<bool> AuthAsync(LoginUserDto loginUserDto)
    {
        var email = loginUserDto.Email ?? throw new NullReferenceException("The email must be set.");
        var password = loginUserDto.Password ?? throw new NullReferenceException("The password must be set.");

        var user = await this.GetAsync(email);

        return _hasher.Compare(password, user.Password!);
    }

    public async Task CreateAsync(User user)
    {
        // create user and save changes
        _repository.User.Create(user);
        await _repository.SaveSafelyAsync();
    }

    public async Task UpdateAsync(User user)
    {
        // update user and save changes
        _repository.User.Update(user);
        await _repository.SaveSafelyAsync();
    }
}
