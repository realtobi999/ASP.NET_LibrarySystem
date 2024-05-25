using LibrarySystem.Application.Contracts.Services;

namespace LibrarySystem.Application.Contracts;

public interface IServiceFactory
{
    IUserService CreateUserService();
}
