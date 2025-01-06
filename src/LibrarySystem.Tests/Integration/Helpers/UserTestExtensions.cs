using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class UserTestExtensions
{
    public static RegisterUserDto ToRegisterUserDto(this User user)
    {
        return new RegisterUserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password
        };
    }
}
