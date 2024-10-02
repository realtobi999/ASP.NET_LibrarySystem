using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class UserMapper : IUserMapper
{
    private readonly IHasher _hasher;

    public UserMapper(IHasher hasher)
    {
        _hasher = hasher;
    }

    public User CreateFromDto(RegisterUserDto dto)
    {
        return new User
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            Password = _hasher.Hash(dto.Password ?? throw new NullReferenceException("The password must be set."))
        };
    }

    public void UpdateFromDto(User user, UpdateUserDto dto)
    {
        user.Email = dto.Email;
        user.Username = dto.Username;
    }
}
