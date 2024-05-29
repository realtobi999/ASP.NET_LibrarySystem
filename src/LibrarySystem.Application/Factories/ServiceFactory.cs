using LibrarySystem.Application.Contracts;
using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Application.Services.Employees;
using LibrarySystem.Application.Services.Users;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Factories;

public class ServiceFactory : IServiceFactory
{
    private readonly IRepositoryManager _repository;
    private readonly IPasswordHasher _hasher;

    public ServiceFactory(IRepositoryManager repository, IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public IEmployeeService CreateEmployeeService()
    {
        return new EmployeeService(_repository, _hasher);
    }

    public IUserService CreateUserService()
    {
        return new UserService(_repository, _hasher);
    }
}