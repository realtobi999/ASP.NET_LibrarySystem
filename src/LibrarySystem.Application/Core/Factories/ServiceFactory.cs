using LibrarySystem.Application.Services.Authors;
using LibrarySystem.Application.Services.Books;
using LibrarySystem.Application.Services.Borrows;
using LibrarySystem.Application.Services.Employees;
using LibrarySystem.Application.Services.Genres;
using LibrarySystem.Application.Services.Pictures;
using LibrarySystem.Application.Services.Reviews;
using LibrarySystem.Application.Services.Users;
using LibrarySystem.Application.Services.Wishlists;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Core.Factories;

public class ServiceFactory : IServiceFactory
{
    private readonly IRepositoryManager _repository;
    private readonly IHasher _hasher;

    public ServiceFactory(IRepositoryManager repository, IHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public IAuthorService CreateAuthorService()
    {
        return new AuthorService(_repository);
    }

    public IBookReviewService CreateBookReviewService()
    {
        return new BookReviewService(_repository);
    }

    public IBookService CreateBookService()
    {
        return new BookService(_repository);
    }

    public IBorrowService CreateBorrowService()
    {
        return new BorrowService(_repository);
    }

    public IEmployeeService CreateEmployeeService()
    {
        return new EmployeeService(_repository, _hasher);
    }

    public IGenreService CreateGenreService()
    {
        return new GenreService(_repository);
    }

    public IPictureService CreatePictureService()
    {
        return new PictureService(_repository);
    }

    public IUserService CreateUserService()
    {
        return new UserService(_repository, _hasher);
    }

    public IWishlistService CreateWishlistService()
    {
        return new WishlistService(_repository);
    }
}