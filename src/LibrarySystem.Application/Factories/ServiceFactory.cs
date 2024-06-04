using LibrarySystem.Application.Contracts;
using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Application.Services.Authors;
using LibrarySystem.Application.Services.Books;
using LibrarySystem.Application.Services.Employees;
using LibrarySystem.Application.Services.Genres;
using LibrarySystem.Application.Services.Users;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Factories;

public class ServiceFactory : IServiceFactory
{
    private readonly IRepositoryManager _repository;
    private readonly IPasswordHasher _hasher;
    private readonly IBookAssociations _bookAssociations;

    public ServiceFactory(IRepositoryManager repository, IPasswordHasher hasher, IBookAssociations bookAssociations)
    {
        _repository = repository;
        _hasher = hasher;
        _bookAssociations = bookAssociations;
    }

    public IAuthorService CreateAuthorService()
    {
        return new AuthorService(_repository);
    }

    public IBookService CreateBookService()
    {
        return new BookService(_repository, _bookAssociations);
    }

    public IEmployeeService CreateEmployeeService()
    {
        return new EmployeeService(_repository, _hasher);
    }

    public IGenreService CreateGenreService()
    {
        return new GenreService(_repository);
    }

    public IUserService CreateUserService()
    {
        return new UserService(_repository, _hasher);
    }
}