using LibrarySystem.Application.Contracts;
using LibrarySystem.Application.Contracts.Services;

namespace LibrarySystem.Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly IServiceFactory _factory;

    public ServiceManager(IServiceFactory factory)
    {
        _factory = factory;
    }

    public IUserService UserService => _factory.CreateUserService();

    public IEmployeeService EmployeeService => _factory.CreateEmployeeService(); 
}