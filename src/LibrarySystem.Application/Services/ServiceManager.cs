using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Interfaces.Services;

namespace LibrarySystem.Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly IServiceFactory _factory;

    public ServiceManager(IServiceFactory factory)
    {
        _factory = factory;
    }

    public IUserService User => _factory.CreateUserService();

    public IEmployeeService Employee => _factory.CreateEmployeeService();

    public IAuthorService Author => _factory.CreateAuthorService();

    public IGenreService Genre => _factory.CreateGenreService();

    public IBookService Book => _factory.CreateBookService();

    public IBorrowService Borrow => _factory.CreateBorrowService();

    public IBookReviewService BookReview => _factory.CreateBookReviewService();

    public IWishlistService Wishlist => _factory.CreateWishlistService(); 
}