using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;

/// <inheritdoc/>
public interface IUserMapper : IMapper<User, RegisterUserDto, UpdateUserDto>
{

}
