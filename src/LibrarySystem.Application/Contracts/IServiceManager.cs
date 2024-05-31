using LibrarySystem.Application.Contracts.Services;

namespace LibrarySystem.Application.Contracts;

public interface IServiceManager
{
    IUserService User { get; }
    IEmployeeService Employee { get; }
}
